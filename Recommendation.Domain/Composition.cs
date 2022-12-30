namespace Recommendation.Domain;

public class Composition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double AverageRating { get; set; }

    public List<Review> Reviews { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
}