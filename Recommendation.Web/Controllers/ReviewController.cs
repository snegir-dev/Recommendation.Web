using System.Collections;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Review.Commands.Create;
using Recommendation.Application.CQs.Review.Commands.Delete;
using Recommendation.Application.CQs.Review.Commands.Update;
using Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;
using Recommendation.Application.CQs.Review.Queries.GetPageReviews;
using Recommendation.Application.CQs.Review.Queries.GetReview;
using Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;
using Recommendation.Web.Models.Review;

namespace Recommendation.Web.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewController : BaseController
{
    public ReviewController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> Get(int numberPage, int pageSize,
        string? searchText, string? filter, string? tag)
    {
        var getPageReviewsQuery = new GetPageReviewsQuery(numberPage, pageSize,
            filter, tag, searchText);
        var getPageReviewsVm = await Mediator.Send(getPageReviewsQuery);

        return Ok(getPageReviewsVm);
    }

    [AllowAnonymous]
    [HttpGet("{reviewId:guid}")]
    public async Task<ActionResult<IEnumerable<GetReviewDto>>> Get(Guid reviewId)
    {
        var getReviewQuery = new GetReviewQuery(UserId, reviewId);
        var review = await Mediator.Send(getReviewQuery);

        return Ok(review);
    }

    [HttpGet("get-updated-review/{reviewId:guid}")]
    public async Task<ActionResult<GetUpdatedReviewDto>> GetUpdatedReview(Guid reviewId)
    {
        var getUpdatedReviewQuery = new GetUpdatedReviewQuery(reviewId);
        var reviewDto = await Mediator.Send(getUpdatedReviewQuery);

        return Ok(reviewDto);
    }

    [HttpGet("get-by-user-id/{userId:guid}")]
    public async Task<ActionResult<IEnumerable
        <GetAllReviewByUserIdDto>>> GetByUserId(Guid userId)
    {
        var getAllReviewByUserIdDtoQuery = new GetAllReviewByUserIdQuery(userId);
        var reviews = await Mediator.Send(getAllReviewByUserIdDtoQuery);

        return Ok(reviews);
    }

    [HttpPost, DisableRequestSizeLimit]
    public async Task<ActionResult<Guid>> Create([FromForm] CreateReviewDto reviewDto)
    {
        var createReviewCommand = Mapper.Map<CreateReviewCommand>(reviewDto);
        createReviewCommand.UserId = UserId;
        var reviewId = await Mediator.Send(createReviewCommand);

        return Created("api/reviews", reviewId);
    }

    [HttpPut, DisableRequestSizeLimit]
    public async Task<ActionResult> Update([FromForm] UpdatedReviewDto reviewDto)
    {
        var updateReviewQuery = Mapper.Map<UpdateReviewQuery>(reviewDto);
        updateReviewQuery.UserId = UserId;
        await Mediator.Send(updateReviewQuery);

        return Ok();
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<ActionResult> Delete(Guid reviewId)
    {
        var deleteReviewCommand = new DeleteReviewCommand(reviewId);
        await Mediator.Send(deleteReviewCommand);

        return Ok();
    }
}