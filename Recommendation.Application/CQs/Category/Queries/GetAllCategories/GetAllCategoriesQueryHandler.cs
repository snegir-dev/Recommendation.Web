using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.CQs.Tag.Queries.GetAllTags;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Category.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, GetAllCategoriesVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetAllCategoriesVm> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _recommendationDbContext.Categories
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetAllCategoriesVm() { Categories = categories };
    }
}