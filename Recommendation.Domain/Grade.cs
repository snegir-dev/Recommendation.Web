namespace Recommendation.Domain;

public class Grade
{
    public Guid Id { get; set; }
    public int GradeValue { get; set; }

    public List<Review> Reviews { get; set; } = new();
}