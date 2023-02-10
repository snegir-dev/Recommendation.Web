namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsVm
{
    public long TotalCountReviews { get; set; }
    public IEnumerable<GetPageReviewsDto> ReviewDtos { get; set; }
}