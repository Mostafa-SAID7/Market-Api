using MediatR;

namespace Market.API.Features.Vendors.Commands
{
    /// <summary>
    /// Set vendor active status command
    /// </summary>
    public class SetVendorActiveStatusCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Set vendor active status command handler
    /// </summary>
    public class SetVendorActiveStatusCommandHandler : IRequestHandler<SetVendorActiveStatusCommand, VendorResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetVendorActiveStatusCommandHandler> _logger;

        public SetVendorActiveStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<SetVendorActiveStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<VendorResponse> Handle(SetVendorActiveStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting vendor {VendorId} active status to {IsActive}", request.Id, request.IsActive);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(request.Id, cancellationToken);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found");

            vendor.IsActive = request.IsActive;
            await _unitOfWork.Vendors.UpdateAsync(vendor, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return MapToResponse(vendor);
        }

        private VendorResponse MapToResponse(Market.API.Models.Entities.Vendor vendor)
        {
            return new VendorResponse
            {
                Id = vendor.Id,
                UserId = vendor.UserId,
                StoreName = vendor.StoreName,
                StoreDescription = vendor.StoreDescription,
                PhoneNumber = vendor.PhoneNumber,
                CommissionRate = vendor.CommissionRate,
                IsApproved = vendor.IsApproved,
                IsActive = vendor.IsActive,
                AverageRating = vendor.AverageRating,
                TotalReviews = vendor.TotalReviews,
                CreatedAt = vendor.CreatedAt,
                UpdatedAt = vendor.UpdatedAt
            };
        }
    }
}
