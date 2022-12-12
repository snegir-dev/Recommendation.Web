namespace Recommendation.Domain;

public class Composition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
    public List<Rating> Ratings { get; set; } = new();
}