using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Common.Clouds.Firebase.Entities;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Category.Queries.GetCategory;
using Recommendation.Application.CQs.Composition.Commands.GetOrCreate;
using Recommendation.Application.CQs.Rating.Queries.GetAverageRating;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.CQs.Tag.Command.Create;
using Recommendation.Application.CQs.Tag.Queries.GetListTagContainsNames;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Review.Commands.Update;

public class UpdateReviewQueryHandler
    : IRequestHandler<UpdateReviewQuery, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly FirebaseCloud _firebaseCloud;

    public UpdateReviewQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator, IMapper mapper, FirebaseCloud firebaseCloud)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
        _mapper = mapper;
        _firebaseCloud = firebaseCloud;
    }

    public async Task<Unit> Handle(UpdateReviewQuery request,
        CancellationToken cancellationToken)
    {
        await CreateMissingTags(request.Tags);
        var review = await CollectReview(request);
        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Domain.Review> CollectReview(UpdateReviewQuery request)
    {
        var review = await GetReview(request.ReviewId);
        var updatedReview = _mapper.Map(request, review);
        updatedReview.Category = await GetCategory(request.Category);
        updatedReview.Tags = await GetTags(request.Tags);
        updatedReview.Composition = await GetOrCreateComposition(request.NameDescription);
        updatedReview.ImageInfos = await UpdateImage(request.Images, review.ImageInfos);

        return updatedReview;
    }

    private async Task<Domain.Composition> GetOrCreateComposition(string compositionName)
    {
        var getOrCreateCompositionCommand = new GetOrCreateCompositionCommand(compositionName);
        var composition = await _mediator.Send(getOrCreateCompositionCommand);
        
        return composition;
    }

    private async Task<List<ImageInfo>> UpdateImage(IEnumerable<IFormFile> files,
        List<ImageInfo>? imageInfos)
    {
        IEnumerable<ImageMetadata> imageMetadatas;
        if (imageInfos != null && imageInfos.Count > 0)
        {
            imageMetadatas = await _firebaseCloud
                .UpdateFiles(files, imageInfos[0].FolderName);
        }
        else
        {
            imageMetadatas = await _firebaseCloud
                .UploadFiles(files, Guid.NewGuid().ToString());
        }

        return _mapper.Map<IEnumerable<ImageMetadata>, List<ImageInfo>>(imageMetadatas);
    }

    private async Task<List<Domain.Tag>> GetTags(string[] tagNames)
    {
        var getListTagDbContainsNamesQuery = new GetListTagDbContainsNamesQuery(tagNames);
        var tags = await _mediator.Send(getListTagDbContainsNamesQuery);

        return tags.ToList();
    }

    private async Task<Domain.Category> GetCategory(string categoryName)
    {
        var getCategoryDbQuery = new GetCategoryDbQuery(categoryName);
        return await _mediator.Send(getCategoryDbQuery);
    }

    private async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getReviewDbQuery);
        await _recommendationDbContext.Entry(review)
            .IncludesAsync(r => r.User, r => r.ImageInfos!, r => r.Composition, r => r.Tags);
        return review;
    }

    private async Task CreateMissingTags(string[] tags)
    {
        var createHashtagsCommand = new CreateTagsCommand(tags);
        await _mediator.Send(createHashtagsCommand);
    }
}