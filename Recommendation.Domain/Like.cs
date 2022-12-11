namespace Recommendation.Domain;

public class Like
{
    public Guid Id { get; set; }
    public bool IsLike { get; set; }
    
    public UserApp User { get; set; }
    public Review Review { get; set; }
}