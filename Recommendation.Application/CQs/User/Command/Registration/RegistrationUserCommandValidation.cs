using FluentValidation;

namespace Recommendation.Application.CQs.User.Command.Registration;

public class RegistrationUserCommandValidation : AbstractValidator<RegistrationUserCommand>
{
    public RegistrationUserCommandValidation()
    {
        RuleFor(u => u.Login).MinimumLength(5).MaximumLength(100);
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).MinimumLength(5);
        RuleFor(u => u.PasswordConfirmation).Equal(u => u.Password);
    }
}