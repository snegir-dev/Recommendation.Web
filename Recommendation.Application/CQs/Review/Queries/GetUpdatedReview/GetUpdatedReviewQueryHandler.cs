﻿using AutoMapper;
using MediatR;
using Recommendation.Application.Common.Clouds.Firebase.Entities;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

public class GetUpdatedReviewQueryHandler
    : IRequestHandler<GetUpdatedReviewQuery, GetUpdatedReviewDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetUpdatedReviewQueryHandler(IMediator mediator,
        IRecommendationDbContext recommendationDbContext, IMapper mapper)
    {
        _mediator = mediator;
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetUpdatedReviewDto> Handle(GetUpdatedReviewQuery request,
        CancellationToken cancellationToken)
    {
        var review = await GetReview(request.ReviewId, request.UserId, request.Role);
        var reviewDto = _mapper.Map<GetUpdatedReviewDto>(review);
        if (review.ImageInfos != null)
            reviewDto.ImageMetadatas = _mapper.Map<List<ImageInfo>, ImageMetadata[]>(review.ImageInfos);

        return reviewDto;
    }

    private async Task<Domain.Review> GetReview(Guid reviewId, Guid userId, string? role)
    {
        var getUpdatedReviewQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getUpdatedReviewQuery);
        await _recommendationDbContext.Entry(review)
            .IncludesAsync(r => r.User, r => r.ImageInfos!, r => r.Category, r => r.Tags);
        if (role != Role.Admin && review.User.Id != userId)
            throw new AccessDeniedException("Access is denied");

        return review;
    }
}