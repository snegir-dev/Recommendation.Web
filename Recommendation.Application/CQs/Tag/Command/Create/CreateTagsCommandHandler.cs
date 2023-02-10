﻿using MediatR;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Tag.Command.Create;

public class CreateTagsCommandHandler
    : IRequestHandler<CreateTagsCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public CreateTagsCommandHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Unit> Handle(CreateTagsCommand request,
        CancellationToken cancellationToken)
    {
        var missingHashtags = request.Tags
            .Except(_recommendationDbContext.Tags.Select(h => h.Name));
        await CreateTag(missingHashtags, cancellationToken);

        return Unit.Value;
    }

    private async Task CreateTag(IEnumerable<string> tags,
        CancellationToken cancellationToken)
    {
        foreach (var tag in tags)
        {
            await _recommendationDbContext.Tags.AddAsync(new Domain.Tag()
            {
                Name = tag
            }, cancellationToken);
        }

        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
    }
}