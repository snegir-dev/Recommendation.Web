using MediatR;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserDb;

public class GetUserDbQuery : IRequest<UserApp>
{
    public Guid UserId { get; set; }

    public GetUserDbQuery(Guid userId)
    {
        UserId = userId;
    }
}