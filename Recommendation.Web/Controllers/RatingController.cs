using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Rating.Commands.SetRating;
using Recommendation.Web.Models.Grade;

namespace Recommendation.Web.Controllers;

[Route("api/rating")]
public class RatingController : BaseController
{
    public RatingController(IMapper mapper, IMediator mediator) : base(mapper, mediator)
    {
    }

    [HttpPost]
    public async Task<ActionResult<int>> SetRating([FromBody] SetRatingVm setRatingVm)
    {
        var setGradeCommand = Mapper.Map<SetRatingCommand>(setRatingVm);
        setGradeCommand.UserId = UserId;
        await Mediator.Send(setGradeCommand);

        return Ok();
    }
}