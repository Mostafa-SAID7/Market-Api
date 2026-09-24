using Market.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Commands
{
    public class DeleteCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }
    }

    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCartCommandHandler> _logger;

        public DeleteCartCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCartCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCartCommand for user: {UserId}", request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);
            if (cart == null)
                return false;

            await _unitOfWork.Carts.DeleteAsync(cart.Id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}



