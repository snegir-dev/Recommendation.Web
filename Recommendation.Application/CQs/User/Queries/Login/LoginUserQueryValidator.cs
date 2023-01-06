using FluentValidation;

namespace Recommendation.Application.CQs.User.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(u => u.Email).EmailAddress();
    }
}