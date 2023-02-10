using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsQuery : IRequest<GetPageReviewsVm>
{
    public int NumberPage { get; set; }
    public int PageSize { get; set; }
    public string? Filter { get; set; }
    public string? Tag { get; set; }
    public string? SearchValue { get; set; }

    public GetPageReviewsQuery(int numberPage, int pageSize, 
        string? filter, string? tag, string? searchValue)
    {
        NumberPage = numberPage;
        PageSize = pageSize;
        Filter = filter;
        SearchValue = searchValue;
        Tag = tag;
    }
}