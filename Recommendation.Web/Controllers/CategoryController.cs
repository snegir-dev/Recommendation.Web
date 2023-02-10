using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Category;
using Recommendation.Application.CQs.Category.Queries.GetAllCategories;

namespace Recommendation.Web.Controllers;

[Route("api/categories")]
public class CategoryController : BaseController
{
    public CategoryController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
    {
        var getAllCategoriesQuery = new GetAllCategoriesQuery();
        var getAllCategoriesVm = await Mediator.Send(getAllCategoriesQuery);

        return Ok(getAllCategoriesVm.Categories);
    }
}