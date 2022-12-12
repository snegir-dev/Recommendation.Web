using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Comment.Queries.GetComment;

public class GetCommentQueryHandler
    : IRequestHandler<GetCommentQuery, CommentDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetCommentQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(GetCommentQuery request,
        CancellationToken cancellationToken)
    {
        var comment = await _recommendationDbContext.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

        return _mapper.Map<CommentDto>(comment);
    }
}