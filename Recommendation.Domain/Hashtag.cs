namespace Recommendation.Domain;

public class Hashtag
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<Review> Reviews { get; set; }
}