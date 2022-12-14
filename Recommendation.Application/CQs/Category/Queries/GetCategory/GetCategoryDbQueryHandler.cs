using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Category.Queries.GetCategory;

public class GetCategoryDbQueryHandler
    : IRequestHandler<GetCategoryDbQuery, Domain.Category>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetCategoryDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Category> Handle(GetCategoryDbQuery request,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Categories
                   .FirstOrDefaultAsync(c => c.Name == request.Category, cancellationToken)
               ?? throw new NullReferenceException($"The category: '{request.Category}' must not be null");
    }
}