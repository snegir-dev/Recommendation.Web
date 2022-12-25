using MediatR;

namespace Recommendation.Application.CQs.User.Queries.GetUserInfo;

public class GetUserInfoQuery : IRequest<UserInfoDto>
{
    public Guid UserId { get; set; }

    public GetUserInfoQuery(Guid userId)
    {
        UserId = userId;
    }
}