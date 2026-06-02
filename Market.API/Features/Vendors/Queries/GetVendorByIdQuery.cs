using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Vendors.Queries
{
    /// <summary>
    /// Get vendor by id query
    /// </summary>
    public class GetVendorByIdQuery : IRequest<VendorResponse?>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Get vendor by id query handler
    /// </summary>
    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetVendorByIdQueryHandler> _logger;

        public GetVendorByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVendorByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<VendorResponse?> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetVendorByIdQuery for vendor: {VendorId}", request.Id);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(request.Id);
            if (vendor == null)
                return null;

            return new VendorResponse
            {
                Id = vendor.Id,
                UserId = vendor.UserId,
                StoreName = vendor.StoreName,
                StoreDescription = vendor.StoreDescription,
                Logo = vendor.Logo,
                Banner = vendor.Banner,
                PhoneNumber = vendor.PhoneNumber,
                Address = vendor.Address,
                City = vendor.City,
                Country = vendor.Country,
                ZipCode = vendor.ZipCode,
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
