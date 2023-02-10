using MediatR;

namespace Recommendation.Application.CQs.Tag.Command.Create;

public class CreateTagsCommand : IRequest
{
    public string[] Tags { get; set; }

    public CreateTagsCommand(string[] tags)
    {
        Tags = tags;
    }
}