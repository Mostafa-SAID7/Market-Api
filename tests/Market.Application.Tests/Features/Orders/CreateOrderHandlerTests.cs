using Market.Application.Features.Orders.Commands;
using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;
using Microsoft.Extensions.Logging.Abstractions;

namespace Market.Application.Tests.Features.Orders;

/// <summary>
/// Tests for CreateOrderCommandHandler inventory and pricing logic.
/// </summary>
public class CreateOrderHandlerTests : RepositoryTestBase
{
    [Fact]
    public async Task Handle_UsesPersistedProductPriceAndUpdatesInventory()
    {
        var product = new Product { Id = 4, VendorId = 2, Name = "Lamp", Price = 20m, DiscountPrice = 15m, Quantity = 3 };

        var products = CreateMockRepository<IProductRepository>();
        products.Setup(x => x.GetByIdAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        products.Setup(x => x.UpdateAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var orders = CreateMockRepository<IOrderRepository>();
        orders.Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order order, CancellationToken _) => order);

        var users = CreateMockRepository<IUserRepository>();
        users.Setup(x => x.ExistsAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var unitOfWork = CreateMockUnitOfWork();
        unitOfWork.SetupGet(x => x.Users).Returns(users.Object);
        unitOfWork.SetupGet(x => x.Products).Returns(products.Object);
        unitOfWork.SetupGet(x => x.Orders).Returns(orders.Object);
        unitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateOrderCommand
            {
                CustomerId = 9,
                ShippingAddress = "123 Market Street",
                ShippingCost = 2m,
                Tax = 1m,
                Items = [new OrderItemInput { ProductId = 4, Quantity = 2, Price = 0.01m, VendorId = 999, ProductName = "forged" }]
            },
            CancellationToken.None);

        // Verify price override (persisted price used instead of input)
        var item = Assert.Single(response.Items);
        Assert.Equal(15m, item.Price);  // Uses DiscountPrice (15) not input (0.01)
        Assert.Equal("Lamp", item.ProductName);  // Uses persisted name
        Assert.Equal(2, item.VendorId);  // Uses persisted vendor
        
        // Verify calculation
        Assert.Equal(30m, response.SubTotal);  // 2 * 15
        Assert.Equal(33m, response.TotalPrice);  // 30 + 1 (tax) + 2 (shipping)
        
        // Verify inventory update
        Assert.Equal(1, product.Quantity);  // 3 - 2
        Assert.Equal(2, product.Sold);  // +2

        VerifySaveWasCalled(unitOfWork);
    }
}
