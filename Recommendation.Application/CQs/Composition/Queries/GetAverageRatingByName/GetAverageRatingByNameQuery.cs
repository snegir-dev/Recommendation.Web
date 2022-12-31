using MediatR;

namespace Recommendation.Application.CQs.Composition.Queries.GetAverageRatingByName;

public class GetAverageRatingByNameQuery : IRequest<double>
{
    public string CompositionName { get; set; }

    public GetAverageRatingByNameQuery(string compositionName)
    {
        CompositionName = compositionName;
    }
}