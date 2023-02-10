namespace Recommendation.Domain;

public class Comment : IBaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
    
    public UserApp User { get; set; }
    public Review Review { get; set; }
}