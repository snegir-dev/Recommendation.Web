using MediatR;

namespace Recommendation.Application.CQs.Composition.Queries.GetAllComposition;

public class GetAllCompositionQuery : IRequest<IEnumerable<string>>
{
}