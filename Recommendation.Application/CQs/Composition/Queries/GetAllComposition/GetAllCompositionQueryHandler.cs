using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Composition.Queries.GetAllComposition;

public class GetAllCompositionQueryHandler 
    : IRequestHandler<GetAllCompositionQuery, IEnumerable<string>>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetAllCompositionQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<IEnumerable<string>> Handle(GetAllCompositionQuery request,
        CancellationToken cancellationToken)
    {
        var compositions = await _recommendationDbContext.Compositions
            .Select(c => c.Name)
            .ToListAsync(cancellationToken);

        return compositions;
    }
}