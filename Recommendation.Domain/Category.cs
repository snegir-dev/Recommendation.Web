using System.ComponentModel.DataAnnotations;

namespace Recommendation.Domain;

public class Category
{
    public Guid Id { get; set; }
    
    [Key]
    public string Name { get; set; }
    
    public List<Review> Reviews { get; set; }
}