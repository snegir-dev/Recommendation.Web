using MediatR;

namespace Recommendation.Application.CQs.Rating.Commands.SetRating;

public class SetRatingCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; } 
    public int RatingValue { get; set; }
}