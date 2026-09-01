using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Categories.Commands
{
    /// <summary>
    /// Update category command
    /// </summary>
    public class UpdateCategoryCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Update category command handler
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(IMediator mediator, ILogger<UpdateCategoryCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateCategoryCommand for category: {CategoryId}", request.Id);

            var result = await _mediator.Send(
                new UpdateCategoryInternalCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating category
    /// </summary>
    internal class UpdateCategoryInternalCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}



