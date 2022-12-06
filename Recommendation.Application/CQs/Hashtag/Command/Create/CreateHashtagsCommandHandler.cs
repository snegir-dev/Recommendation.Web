using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Hashtag.Command.Create;

public class CreateHashtagsCommandHandler
    : IRequestHandler<CreateHashtagsCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public CreateHashtagsCommandHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Unit> Handle(CreateHashtagsCommand request,
        CancellationToken cancellationToken)
    {
        var hashtags = SplitHashtags(request.HashtagsString);
        var missingHashtags = hashtags
            .Except(_recommendationDbContext.Hashtags.Select(h => h.Name));
        await CreateHashtags(missingHashtags, cancellationToken);

        return Unit.Value;
    }

    private IEnumerable<string> SplitHashtags(string hashtagsString)
    {
        var separatedHashtags = hashtagsString.Split("#");
        var hashtags = separatedHashtags
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Select(h => h.Trim());

        return hashtags;
    }

    private async Task CreateHashtags(IEnumerable<string> hashtags,
        CancellationToken cancellationToken)
    {
        foreach (var hashtag in hashtags)
        {
            await _recommendationDbContext.Hashtags.AddAsync(new Domain.Hashtag()
            {
                Name = hashtag
            }, cancellationToken);
        }

        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
    }
}