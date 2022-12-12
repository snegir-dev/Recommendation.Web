using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsQuery : IRequest<GetPageReviewsVm>
{
    public int NumberPage { get; set; }
    public int PageSize { get; set; }

    public GetPageReviewsQuery(int numberPage, int pageSize)
    {
        NumberPage = numberPage;
        PageSize = pageSize;
    }
}