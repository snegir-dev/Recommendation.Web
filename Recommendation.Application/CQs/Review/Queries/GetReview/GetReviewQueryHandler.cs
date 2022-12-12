using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetReview;

public class GetReviewQueryHandler
    : IRequestHandler<GetReviewQuery, GetReviewDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetReviewQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetReviewDto> Handle(GetReviewQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
            .Include(r => r.User)
            .Include(r => r.Category)
            .Include(r => r.Composition)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);
        var reviewDto = _mapper.Map<GetReviewDto>(review);
        reviewDto.OwnSetRating = await GetOwnSetRating(request.UserId, request.ReviewId);
        reviewDto.IsLike = await GetIsLike(request.UserId, request.ReviewId);
        reviewDto.AverageCompositionRate = await GetAverageCompositionRating(request.ReviewId);

        return reviewDto;
    }

    private async Task<double> GetAverageCompositionRating(Guid reviewId)
    {
        var averageCompositionRate = await _recommendationDbContext.Ratings
            .Where(r => r.Composition.ReviewId == reviewId)
            .Select(r => r.RatingValue)
            .DefaultIfEmpty()
            .AverageAsync();

        return averageCompositionRate;
    }

    private async Task<bool> GetIsLike(Guid userId, Guid reviewId)
    {
        var like = await _recommendationDbContext.Likes
                       .Include(g => g.User)
                       .FirstOrDefaultAsync(g => g.User.Id == userId &&
                                                 g.Review.Id == reviewId)
                   ?? new Domain.Like();
        return like.IsLike;
    }

    private async Task<int> GetOwnSetRating(Guid userId, Guid reviewId)
    {
        var rating = await _recommendationDbContext.Ratings
                         .Include(g => g.User)
                         .FirstOrDefaultAsync(g => g.User.Id == userId &&
                                                   g.Composition.ReviewId == reviewId)
                     ?? new Domain.Rating();
        return rating.RatingValue;
    }
}