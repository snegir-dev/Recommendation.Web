using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Tag.Queries.GetAllTags;

namespace Recommendation.Web.Controllers;

[Route("api/tags")]
public class TagController : BaseController
{
    public TagController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllTagsDto>>> Get()
    {
        var getAllTagsQuery = new GetAllTagsQuery();
        var allTagsVm = await Mediator.Send(getAllTagsQuery);

        return Ok(allTagsVm.Tags);
    }
}