using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Categories.Queries
{
    /// <summary>
    /// Get category by id query
    /// </summary>
    public class GetCategoryByIdQuery : IRequest<CategoryResponse?>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Get category by id query handler
    /// </summary>
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCategoryByIdQuery for category: {CategoryId}", request.Id);

            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (category == null)
                return null;

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SlugValue = category.SlugValue,
                IsActive = category.IsActive,
                DisplayOrder = category.DisplayOrder
            };
        }
    }
}
