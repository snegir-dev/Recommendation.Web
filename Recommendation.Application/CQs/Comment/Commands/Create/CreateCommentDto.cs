namespace Recommendation.Application.CQs.Comment.Commands.Create;

public class CreateCommentDto
{
    public string AuthorName { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
}