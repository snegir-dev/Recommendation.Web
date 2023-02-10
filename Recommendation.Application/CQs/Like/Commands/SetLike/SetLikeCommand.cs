using MediatR;

namespace Recommendation.Application.CQs.Like.Commands.SetLike;

public class SetLikeCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public bool IsLike { get; set; }
}