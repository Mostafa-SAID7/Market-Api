using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Vendors.Queries
{
    /// <summary>
    /// Get all vendors query
    /// </summary>
    public class GetAllVendorsQuery : IRequest<List<VendorResponse>>
    {
    }

    /// <summary>
    /// Get all vendors query handler
    /// </summary>
    public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, List<VendorResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllVendorsQueryHandler> _logger;

        public GetAllVendorsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllVendorsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<VendorResponse>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllVendorsQuery");

            var vendors = await _unitOfWork.Vendors.GetAllAsync();
            return vendors.Select(v => new VendorResponse
            {
                Id = v.Id,
                UserId = v.UserId,
                StoreName = v.StoreName,
                StoreDescription = v.StoreDescription,
                Logo = v.Logo,
                Banner = v.Banner,
                PhoneNumber = v.PhoneNumber,
                Address = v.Address,
                City = v.City,
                Country = v.Country,
                ZipCode = v.ZipCode,
                CommissionRate = v.CommissionRate,
                IsApproved = v.IsApproved,
                IsActive = v.IsActive,
                AverageRating = v.AverageRating,
                TotalReviews = v.TotalReviews,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            }).ToList();
        }
    }
}
