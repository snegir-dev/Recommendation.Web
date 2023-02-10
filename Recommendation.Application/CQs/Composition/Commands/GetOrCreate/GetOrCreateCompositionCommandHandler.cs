using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Composition.Commands.GetOrCreate;

public class GetOrCreateCompositionCommandHandler
    : IRequestHandler<GetOrCreateCompositionCommand, Domain.Composition>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetOrCreateCompositionCommandHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Composition> Handle(GetOrCreateCompositionCommand request,
        CancellationToken cancellationToken)
    {
        var composition = await _recommendationDbContext.Compositions
            .FirstOrDefaultAsync(c => c.Name.ToLower() == 
                                      request.CompositionName.ToLower(), cancellationToken);
        if (composition != null)
            return composition;

        return await Create(request.CompositionName, cancellationToken);
    }

    private async Task<Domain.Composition> Create(string compositionName,
        CancellationToken cancellationToken)
    {
        var composition = new Domain.Composition()
        {
            Name = compositionName
        };
        await _recommendationDbContext.Compositions.AddAsync(composition, cancellationToken);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return composition;
    }
}