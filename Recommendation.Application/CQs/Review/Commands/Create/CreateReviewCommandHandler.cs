﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Common.Clouds.Firebase.Entities;
using Recommendation.Application.CQs.Category.Queries.GetCategory;
using Recommendation.Application.CQs.Composition.Commands.GetOrCreate;
using Recommendation.Application.CQs.Tag.Command.Create;
using Recommendation.Application.CQs.Tag.Queries.GetListTagContainsNames;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Review.Commands.Create;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IFirebaseCloud _firebaseCloud;

    public CreateReviewCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IMediator mediator, IFirebaseCloud firebaseCloud)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _mediator = mediator;
        _firebaseCloud = firebaseCloud;
    }

    public async Task<Guid> Handle(CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        await CreateMissingTags(request.Tags);
        var review = await CollectReview(request);
        await _recommendationDbContext.Reviews.AddAsync(review, cancellationToken);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return review.Id;
    }

    private async Task<Domain.Review> CollectReview(CreateReviewCommand request)
    {
        var review = _mapper.Map<Domain.Review>(request);
        review.User = await GetUser(request.UserId);
        review.Tags = await GetTags(request.Tags);
        review.Category = await GetCategory(request.Category);
        if (request.Images != null)
            review.ImageInfos = await UploadImages(request.Images);
        review.Composition = await GetOrCreateComposition(request.NameDescription);

        return review;
    }

    private async Task<Domain.Composition> GetOrCreateComposition(string compositionName)
    {
        var getOrCreateCompositionCommand = new GetOrCreateCompositionCommand(compositionName);
        return await _mediator.Send(getOrCreateCompositionCommand);
    }

    private async Task<List<ImageInfo>> UploadImages(IEnumerable<IFormFile> files)
    {
        var imageMetadatas = await _firebaseCloud
            .UploadFilesAsync(files, Guid.NewGuid().ToString());
        var imageInfos = _mapper.Map<IEnumerable<ImageMetadata>,
            List<ImageInfo>>(imageMetadatas);

        return imageInfos;
    }

    private async Task<UserApp> GetUser(Guid userId)
    {
        var getUserDbQuery = new GetUserDbQuery(userId);
        return await _mediator.Send(getUserDbQuery);
    }

    private async Task<List<Domain.Tag>> GetTags(string[] tagNames)
    {
        var getListTagDbContainsNamesQuery = new GetListTagDbContainsNamesQuery(tagNames);
        var tags = await _mediator.Send(getListTagDbContainsNamesQuery);

        return tags.ToList();
    }

    private async Task<Domain.Category> GetCategory(string category)
    {
        var getCategoryDbQuery = new GetCategoryDbQuery(category);
        return await _mediator.Send(getCategoryDbQuery);
    }

    private async Task CreateMissingTags(string[] tags)
    {
        var createHashtagsCommand = new CreateTagsCommand(tags);
        await _mediator.Send(createHashtagsCommand);
    }
}