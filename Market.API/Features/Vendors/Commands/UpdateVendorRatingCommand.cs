using MediatR;

namespace Market.API.Features.Vendors.Commands
{
    /// <summary>
    /// Update vendor rating command
    /// </summary>
    public class UpdateVendorRatingCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }

    /// <summary>
    /// Update vendor rating command handler
    /// </summary>
    public class UpdateVendorRatingCommandHandler : IRequestHandler<UpdateVendorRatingCommand, VendorResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateVendorRatingCommandHandler> _logger;

        public UpdateVendorRatingCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateVendorRatingCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<VendorResponse> Handle(UpdateVendorRatingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating vendor {VendorId} rating to {AverageRating} with {TotalReviews} total reviews", 
                request.Id, request.AverageRating, request.TotalReviews);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(request.Id, cancellationToken);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found");

            vendor.AverageRating = request.AverageRating;
            vendor.TotalReviews = request.TotalReviews;
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
