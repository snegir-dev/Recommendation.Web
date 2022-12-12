namespace Recommendation.Domain;

public class Comment
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
    
    public UserApp User { get; set; }
    public Review Review { get; set; }
}