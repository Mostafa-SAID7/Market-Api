using Market.Application.Features.Carts.Commands;
using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;
using Microsoft.Extensions.Logging.Abstractions;

namespace Market.Application.Tests.Features.Carts;

/// <summary>
/// Tests for UpdateCartItemQuantityCommandHandler business logic.
/// </summary>
public class UpdateCartItemQuantityHandlerTests : RepositoryTestBase
{
    [Fact]
    public async Task Handle_WithExistingItem_UpdatesAndPersistsCart()
    {
        var cart = new Cart
        {
            UserId = 9,
            Items = [new CartItem { ProductId = 4, VendorId = 2, ProductName = "Lamp", Price = 12m, Quantity = 1 }]
        };

        var carts = CreateMockRepository<ICartRepository>();
        carts.Setup(x => x.GetByUserIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        carts.Setup(x => x.UpdateAsync(cart, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var unitOfWork = CreateMockUnitOfWork();
        unitOfWork.SetupGet(x => x.Carts).Returns(carts.Object);
        unitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new UpdateCartItemQuantityCommandHandler(unitOfWork.Object, NullLogger<UpdateCartItemQuantityCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateCartItemQuantityCommand { UserId = 9, ProductId = 4, Quantity = 3 },
            CancellationToken.None);

        Assert.Equal(3, Assert.Single(response.Items).Quantity);
        VerifySaveWasCalled(unitOfWork);
    }

    [Fact]
    public async Task Handle_WithNonPositiveQuantity_RejectsBeforePersistence()
    {
        var unitOfWork = CreateMockUnitOfWork();
        var handler = new UpdateCartItemQuantityCommandHandler(unitOfWork.Object, NullLogger<UpdateCartItemQuantityCommandHandler>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new UpdateCartItemQuantityCommand { UserId = 9, ProductId = 4, Quantity = 0 }, CancellationToken.None));

        VerifySaveWasNotCalled(unitOfWork);
    }
}
