using Market.Application.Features.Orders.Commands;
using Microsoft.Extensions.Logging.Abstractions;

namespace Market.Application.Tests.Features.Orders;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_UsesPersistedProductValuesAndUpdatesInventory()
    {
        var product = new Product
        {
            Id = 10,
            Name = "Persisted product",
            VendorId = 7,
            Price = 25m,
            DiscountPrice = 20m,
            Quantity = 5,
            Sold = 2,
            Status = ProductStatus.Active
        };
        var users = new Mock<IUserRepository>(MockBehavior.Strict);
        var products = new Mock<IProductRepository>(MockBehavior.Strict);
        var orders = new Mock<IOrderRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        users.Setup(repository => repository.ExistsAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        products.Setup(repository => repository.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        products.Setup(repository => repository.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        orders.Setup(repository => repository.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order order, CancellationToken _) => order);
        unitOfWork.SetupGet(value => value.Users).Returns(users.Object);
        unitOfWork.SetupGet(value => value.Products).Returns(products.Object);
        unitOfWork.SetupGet(value => value.Orders).Returns(orders.Object);
        unitOfWork.Setup(value => value.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);
        var command = new CreateOrderCommand
        {
            CustomerId = 3,
            ShippingAddress = "123 Market Street",
            ShippingCost = 4m,
            Tax = 2m,
            Items = [new OrderItemInput { ProductId = 10, ProductName = "Forged", VendorId = 99, Price = .01m, Quantity = 2 }]
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(3, product.Quantity);
        Assert.Equal(4, product.Sold);
        var item = Assert.Single(result.Items);
        Assert.Equal("Persisted product", item.ProductName);
        Assert.Equal(7, item.VendorId);
        Assert.Equal(20m, item.Price);
        Assert.Equal(40m, result.SubTotal);
        Assert.Equal(46m, result.TotalPrice);
        unitOfWork.Verify(value => value.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotHaveSufficientStock_DoesNotPersistOrder()
    {
        var users = new Mock<IUserRepository>(MockBehavior.Strict);
        var products = new Mock<IProductRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        users.Setup(repository => repository.ExistsAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        products.Setup(repository => repository.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = 10, Quantity = 1, Status = ProductStatus.Active });
        unitOfWork.SetupGet(value => value.Users).Returns(users.Object);
        unitOfWork.SetupGet(value => value.Products).Returns(products.Object);
        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateOrderCommand
        {
            CustomerId = 3,
            ShippingAddress = "123 Market Street",
            Items = [new OrderItemInput { ProductId = 10, Quantity = 2 }]
        }, CancellationToken.None));

        unitOfWork.Verify(value => value.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_DoesNotPersistOrder()
    {
        var users = new Mock<IUserRepository>(MockBehavior.Strict);
        var products = new Mock<IProductRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        users.Setup(repository => repository.ExistsAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        products.Setup(repository => repository.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);
        unitOfWork.SetupGet(value => value.Users).Returns(users.Object);
        unitOfWork.SetupGet(value => value.Products).Returns(products.Object);
        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(new CreateOrderCommand
        {
            CustomerId = 3,
            ShippingAddress = "123 Market Street",
            Items = [new OrderItemInput { ProductId = 10, Quantity = 1 }]
        }, CancellationToken.None));

        unitOfWork.Verify(value => value.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
