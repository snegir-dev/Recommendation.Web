using MediatR;

namespace Recommendation.Application.CQs.Tag.Command.Create;

public class CreateHashtagsCommand : IRequest
{
    public string[] Tags { get; set; }

    public CreateHashtagsCommand(string[] tags)
    {
        Tags = tags;
    }
}