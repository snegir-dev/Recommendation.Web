using MediatR;

namespace Recommendation.Application.CQs.Hashtag.Command.Create;

public class CreateHashtagsCommand : IRequest
{
    public string HashtagsString { get; set; }
}