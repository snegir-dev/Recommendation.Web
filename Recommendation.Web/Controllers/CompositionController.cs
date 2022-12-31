using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Composition.Queries.GetAllComposition;

namespace Recommendation.Web.Controllers;

[Route("api/compositions")]
public class CompositionController : BaseController
{
    public CompositionController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {
        var getAllCompositionQuery = new GetAllCompositionQuery();
        var compositions = await Mediator.Send(getAllCompositionQuery);

        return Ok(compositions);
    }
}