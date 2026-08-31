using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetReviewByIdQuery : IRequest<ReviewResponse?>
    {
        public int Id { get; set; }
    }

    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewResponse?>
    {
        private readonly ILogger<GetReviewByIdQueryHandler> _logger;

        public GetReviewByIdQueryHandler(ILogger<GetReviewByIdQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<ReviewResponse?> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewByIdQuery for id: {Id}", request.Id);
            return null;
        }
    }
}
