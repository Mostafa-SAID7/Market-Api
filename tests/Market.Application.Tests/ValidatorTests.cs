using Market.Application.Validators;
using Market.Application.Features.Carts.Commands;
using Market.Application.Features.Orders.Commands;
using Market.Domain.Entities;
using Market.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Market.Application.Tests;

public class ValidatorTests
{
    [Fact]
    public void ProductValidator_ValidProduct_Passes()
    {
        var result = new ProductValidator().Validate(new Product
        {
            Name = "Desk lamp", Description = "An adjustable desk lamp.", VendorId = 1,
            CategoryId = 1, Price = 20m, Quantity = 0, AverageRating = 5m
        });
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ProductValidator_RejectsInvalidPriceDiscountAndRating()
    {
        var result = new ProductValidator().Validate(new Product
        {
            Name = " ", Description = "short", VendorId = 0, CategoryId = 0,
            Price = 0m, DiscountPrice = 0m, Quantity = -1, AverageRating = 5.01m
        });
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == nameof(Product.Price));
        Assert.Contains(result.Errors, e => e.Field == nameof(Product.AverageRating));
    }

    [Theory]
    [InlineData(0.00)]
    [InlineData(0.50)]
    public void VendorValidator_AcceptsDatabaseCommissionBoundaries(decimal rate)
    {
        var result = new VendorValidator().Validate(ValidVendor(rate));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void VendorValidator_RejectsCommissionRateAboveDatabaseConstraint()
    {
        var result = new VendorValidator().Validate(ValidVendor(0.51m));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == nameof(Vendor.CommissionRate));
    }

    [Fact]
    public void ReviewValidator_RejectsInvalidRatingAndUnsafeImageUrl()
    {
        var review = new Review { ProductId = 1, CustomerId = 1, VendorId = 1, RatingValue = 6, Title = "Bad", Comment = "Long enough comment", Images = [new ReviewImage { ImageUrl = "file:///etc/passwd" }] };
        var result = new ReviewValidator().Validate(review);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == nameof(Review.RatingValue));
        Assert.Contains(result.Errors, e => e.Field == nameof(Review.Images));
    }

    [Fact]
    public async Task UpdateCartItemQuantityHandler_ExistingItem_UpdatesAndPersistsCart()
    {
        var cart = new Cart
        {
            UserId = 9,
            Items = [new CartItem { ProductId = 4, VendorId = 2, ProductName = "Lamp", Price = 12m, Quantity = 1 }]
        };
        var carts = new Mock<ICartRepository>(MockBehavior.Strict);
        carts.Setup(x => x.GetByUserIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        carts.Setup(x => x.UpdateAsync(cart, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(x => x.Carts).Returns(carts.Object);
        unitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new UpdateCartItemQuantityCommandHandler(unitOfWork.Object, NullLogger<UpdateCartItemQuantityCommandHandler>.Instance);

        var response = await handler.Handle(new UpdateCartItemQuantityCommand { UserId = 9, ProductId = 4, Quantity = 3 }, CancellationToken.None);

        Assert.Equal(3, Assert.Single(response.Items).Quantity);
        carts.Verify(x => x.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCartItemQuantityHandler_NonPositiveQuantity_RejectsBeforePersistence()
    {
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var handler = new UpdateCartItemQuantityCommandHandler(unitOfWork.Object, NullLogger<UpdateCartItemQuantityCommandHandler>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new UpdateCartItemQuantityCommand { UserId = 9, ProductId = 4, Quantity = 0 }, CancellationToken.None));
        unitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateOrderHandler_UsesPersistedProductPriceAndUpdatesInventory()
    {
        var product = new Product { Id = 4, VendorId = 2, Name = "Lamp", Price = 20m, DiscountPrice = 15m, Quantity = 3 };
        var products = new Mock<IProductRepository>(MockBehavior.Strict);
        products.Setup(x => x.GetByIdAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        products.Setup(x => x.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var orders = new Mock<IOrderRepository>(MockBehavior.Strict);
        orders.Setup(x => x.CreateAsync(It.IsAny<Market.Domain.Entities.Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Market.Domain.Entities.Order order, CancellationToken _) => order);
        var users = new Mock<IUserRepository>(MockBehavior.Strict);
        users.Setup(x => x.ExistsAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(x => x.Users).Returns(users.Object);
        unitOfWork.SetupGet(x => x.Products).Returns(products.Object);
        unitOfWork.SetupGet(x => x.Orders).Returns(orders.Object);
        unitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);

        var response = await handler.Handle(new CreateOrderCommand
        {
            CustomerId = 9, ShippingAddress = "123 Market Street", ShippingCost = 2m, Tax = 1m,
            Items = [new OrderItemInput { ProductId = 4, Quantity = 2, Price = 0.01m, VendorId = 999, ProductName = "forged" }]
        }, CancellationToken.None);

        var item = Assert.Single(response.Items);
        Assert.Equal(15m, item.Price);
        Assert.Equal("Lamp", item.ProductName);
        Assert.Equal(2, item.VendorId);
        Assert.Equal(30m, response.SubTotal);
        Assert.Equal(33m, response.TotalPrice);
        Assert.Equal(1, product.Quantity);
        Assert.Equal(2, product.Sold);
        unitOfWork.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Vendor ValidVendor(decimal rate) => new()
    {
        UserId = 1, StoreName = "Good Store", StoreDescription = "A good store description.", CommissionRate = rate, AverageRating = 5m
    };
}
