using AutoMapper;
using MediatR;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

public class GetUpdatedReviewQueryHandler
    : IRequestHandler<GetUpdatedReviewQuery, GetUpdatedReviewDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetUpdatedReviewQueryHandler(IMediator mediator,
        IRecommendationDbContext recommendationDbContext, IMapper mapper)
    {
        _mediator = mediator;
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetUpdatedReviewDto> Handle(GetUpdatedReviewQuery request,
        CancellationToken cancellationToken)
    {
        var review = await GetReview(request.ReviewId);
        var reviewDto = _mapper.Map<GetUpdatedReviewDto>(review);

        return reviewDto;
    }

    private async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getUpdatedReviewQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getUpdatedReviewQuery);
        await _recommendationDbContext.Entry(review).Reference(r => r.Category).LoadAsync();
        await _recommendationDbContext.Entry(review).Collection(r => r.Tags).LoadAsync();

        return review;
    }
}