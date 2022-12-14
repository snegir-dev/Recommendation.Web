using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Tag.Queries.GetListTagContainsNames;

public class GetListTagDbContainsNamesQueryHandler
    : IRequestHandler<GetListTagDbContainsNamesQuery, IEnumerable<Domain.Tag>>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetListTagDbContainsNamesQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<IEnumerable<Domain.Tag>> Handle(GetListTagDbContainsNamesQuery request, 
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Tags
            .Where(h => request.Tags.Contains(h.Name))
            .ToListAsync(cancellationToken);
    }
}