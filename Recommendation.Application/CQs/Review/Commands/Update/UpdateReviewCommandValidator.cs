using FluentValidation;

namespace Recommendation.Application.CQs.Review.Commands.Update;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(ur => ur.NameReview).MinimumLength(5).MaximumLength(100);
        RuleFor(ur => ur.NameDescription).MinimumLength(5).MaximumLength(100);
        RuleFor(ur => ur.Description).MinimumLength(100).MaximumLength(10000);
        RuleFor(ur => ur.Category).NotEmpty();
        RuleFor(ur => ur.Tags).NotEmpty();
    }
}