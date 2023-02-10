using FluentValidation;

namespace Recommendation.Application.CQs.Review.Commands.Create;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(cr => cr.NameReview).MinimumLength(5).MaximumLength(100);
        RuleFor(cr => cr.NameDescription).MinimumLength(5).MaximumLength(100);
        RuleFor(cr => cr.Description).MinimumLength(100).MaximumLength(10000);
        RuleFor(cr => cr.Category).NotEmpty();
        RuleFor(cr => cr.Tags).NotEmpty();
    }
}