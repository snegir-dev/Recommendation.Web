using MediatR;

namespace Recommendation.Application.CQs.Composition.Queries.GetCompositionDb;

public class GetCompositionDbQuery : IRequest<Domain.Composition>
{
    public Guid CompositionId { get; set; }

    public GetCompositionDbQuery(Guid compositionId)
    {
        CompositionId = compositionId;
    }
}