using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Composition.Queries.GetCompositionDb;

public class GetCompositionDbQueryHandler
    : IRequestHandler<GetCompositionDbQuery, Domain.Composition>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetCompositionDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Composition> Handle(GetCompositionDbQuery request,
        CancellationToken cancellationToken)
    {
        var composition = await _recommendationDbContext.Compositions
            .FirstOrDefaultAsync(c => c.Id == request.CompositionId, cancellationToken);
        if (composition == null)
            throw new NotFoundException(nameof(Domain.Composition), request.CompositionId);

        return composition;
    }
}