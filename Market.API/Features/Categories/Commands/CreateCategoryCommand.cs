using MediatR;
using Market.API.Models.Entities;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Categories.Commands
{
    /// <summary>
    /// Create category command
    /// </summary>
    public class CreateCategoryCommand : IRequest<CategoryResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Create category command handler
    /// </summary>
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateCategoryCommand for category: {CategoryName}", request.Name);

            var category = Category.Create(request.Name, request.Description, request.ImageUrl);

            await _unitOfWork.Categories.CreateAsync(category);
            await _unitOfWork.SaveAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SlugValue = category.SlugValue,
                IsActive = category.IsActive
            };
        }
    }
}
