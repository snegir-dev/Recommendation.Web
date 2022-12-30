using MediatR;

namespace Recommendation.Application.CQs.Composition.Commands.GetOrCreate;

public class GetOrCreateCompositionCommand : IRequest<Domain.Composition>
{
    public string CompositionName { get; set; }
    
    public GetOrCreateCompositionCommand(string compositionName)
    {
        CompositionName = compositionName;
    }
}