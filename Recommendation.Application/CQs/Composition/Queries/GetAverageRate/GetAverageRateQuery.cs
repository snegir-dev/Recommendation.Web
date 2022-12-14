using MediatR;

namespace Recommendation.Application.CQs.Composition.Queries.GetAverageRate;

public class GetAverageRateQuery : IRequest<double>
{
    public Guid ReviewId { get; set; }

    public GetAverageRateQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}