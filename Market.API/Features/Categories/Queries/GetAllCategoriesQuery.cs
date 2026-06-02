using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Categories.Queries
{
    /// <summary>
    /// Get all categories query
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<List<CategoryResponse>>
    {
    }

    /// <summary>
    /// Get all categories query handler
    /// </summary>
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllCategoriesQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllCategoriesQuery");

            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SlugValue = c.SlugValue,
                IsActive = c.IsActive,
                DisplayOrder = c.DisplayOrder
            }).ToList();
        }
    }
}
