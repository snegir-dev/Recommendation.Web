using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation.Domain;

public class Review
{
    public Guid Id { get; set; }
    public string UrlImage { get; set; }
    public string NameReview { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public DateTime DateCreation { get; set; }

    public UserApp User { get; set; }
    public Category Category { get; set; }
    public Composition Composition { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}