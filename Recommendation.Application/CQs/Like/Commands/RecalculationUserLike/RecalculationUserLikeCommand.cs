using MediatR;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Like.Commands.RecalculationUserLike;

public class RecalculationUserLikeCommand : IRequest
{
    public Guid UserId { get; set; }

    public RecalculationUserLikeCommand(Guid userId)
    {
        UserId = userId;
    }
}