using MediatR;

namespace Market.API.Features.Categories.Commands
{
    /// <summary>
    /// Set category active status command
    /// </summary>
    public class SetCategoryActiveStatusCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Set category active status command handler
    /// </summary>
    public class SetCategoryActiveStatusCommandHandler : IRequestHandler<SetCategoryActiveStatusCommand, CategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetCategoryActiveStatusCommandHandler> _logger;

        public SetCategoryActiveStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<SetCategoryActiveStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CategoryResponse> Handle(SetCategoryActiveStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting category {CategoryId} active status to {IsActive}", request.Id, request.IsActive);

            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found");

            category.IsActive = request.IsActive;
            await _unitOfWork.Categories.UpdateAsync(category, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return MapToResponse(category);
        }

        private CategoryResponse MapToResponse(Market.API.Models.Entities.Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                DisplayOrder = category.DisplayOrder,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }
    }
}
