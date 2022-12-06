using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation.Domain;

public class Review
{
    public Guid Id { get; set; }
    public string UrlImage { get; set; }
    public string ReviewName { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int Grade { get; set; }

    public Category Category { get; set; }
    public List<Hashtag> Hashtags { get; set; }
}