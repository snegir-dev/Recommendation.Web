namespace Recommendation.Domain;

public class Rating
{
    public Guid Id { get; set; }
    public int RatingValue { get; set; }

    public UserApp User { get; set; }
    public Composition Composition { get; set; } = new();
}