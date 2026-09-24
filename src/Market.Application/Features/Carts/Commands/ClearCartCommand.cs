using Market.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Commands
{
    /// <summary>
    /// Clear cart command
    /// </summary>
    public class ClearCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Clear cart command handler
    /// </summary>
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClearCartCommandHandler> _logger;

        public ClearCartCommandHandler(IUnitOfWork unitOfWork, ILogger<ClearCartCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ClearCartCommand for user: {UserId}", request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);
            if (cart == null)
                return false;

            // Clear all items from cart
            cart.Clear();

            // Update cart
            await _unitOfWork.Carts.UpdateAsync(cart, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}



