using Market.Domain.Enums;
using Market.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Orders.Commands
{
    /// <summary>
    /// Delete order command
    /// </summary>
    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Delete order command handler
    /// </summary>
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteOrderCommand for order: {OrderId}", request.Id);

            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                return false;

            // Only allow deletion of pending or cancelled orders
            if (order.OrderStatus != OrderStatus.Pending && order.OrderStatus != OrderStatus.Cancelled)
            {
                _logger.LogWarning("Cannot delete order {OrderId} with status {Status}", request.Id, order.OrderStatus);
                return false;
            }

            // Restore inventory before deleting
            foreach (var item in order.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId, cancellationToken);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    product.Sold -= item.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
                }
            }

            await _unitOfWork.Orders.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}



