using FluentValidation;

namespace Recommendation.Application.CQs.Comment.Commands.Create;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(cc => cc.Comment).NotEmpty();
    }
}