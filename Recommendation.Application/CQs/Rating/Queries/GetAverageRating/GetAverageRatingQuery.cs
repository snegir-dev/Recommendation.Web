using MediatR;

namespace Recommendation.Application.CQs.Rating.Queries.GetAverageRating;

public class GetAverageRatingQuery : IRequest<double>
{
    public Guid CompositionId { get; set; }
    public int? AdditionalRating { get; set; }

    public GetAverageRatingQuery(Guid compositionId, int? additionalRating = null)
    {
        CompositionId = compositionId;
        AdditionalRating = additionalRating;
    }
}