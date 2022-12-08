using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Review.Create;
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

    [HttpPost, DisableRequestSizeLimit]
    public async Task<ActionResult> Create([FromForm] CreateReviewDto reviewDto)
    {
        var createReviewCommand = Mapper.Map<CreateReviewCommand>(reviewDto);
        createReviewCommand.UserId = UserId;
        var reviewId = await Mediator.Send(createReviewCommand);

        return Created("api/reviews", reviewId);
    }
}