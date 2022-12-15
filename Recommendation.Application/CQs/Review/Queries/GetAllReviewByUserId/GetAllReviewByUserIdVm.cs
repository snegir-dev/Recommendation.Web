namespace Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;

public class GetAllReviewByUserIdVm
{
    public IEnumerable<GetAllReviewByUserIdDto> Reviews { get; set; }

    public GetAllReviewByUserIdVm(IEnumerable<GetAllReviewByUserIdDto> reviews)
    {
        Reviews = reviews;
    }
}