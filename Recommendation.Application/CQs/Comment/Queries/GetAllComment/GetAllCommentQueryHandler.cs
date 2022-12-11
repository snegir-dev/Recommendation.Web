using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Comment.Queries.GetAllComment;

public class GetAllCommentQueryHandler
    : IRequestHandler<GetAllCommentQuery, IEnumerable<GetAllCommentDto>>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetAllCommentQueryHandler(IRecommendationDbContext recommendationDbContext, 
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllCommentDto>> Handle(GetAllCommentQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await _recommendationDbContext.Comments
            .Include(c => c.Review)
            .Where(c => c.Review.Id == request.ReviewId)
            .OrderBy(c => c.DateCreation)
            .ProjectTo<GetAllCommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return comments;
    }
}