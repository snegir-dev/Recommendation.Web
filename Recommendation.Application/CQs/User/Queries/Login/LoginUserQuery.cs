using MediatR;

namespace Recommendation.Application.CQs.User.Queries.Login;

public class LoginUserQuery : IRequest
{
    public string Email { get; set; }
    public string Password { set; get; }
    public bool IsRemember { set; get; }
}