using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Comment;
using Recommendation.Application.CQs.Comment.Commands.Create;
using Recommendation.Application.CQs.Comment.Queries.GetAllComment;
using Recommendation.Web.Models.Comment;

namespace Recommendation.Web.Controllers;

[Route("api/comments")]
public class CommentController : BaseController
{
    public CommentController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult
        <IEnumerable<CommentDto>>> Get([FromQuery] Guid reviewId)
    {
        var getAllCommentQuery = new GetAllCommentQuery(reviewId);
        var comments = await Mediator.Send(getAllCommentQuery);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCommendVm commendVm)
    {
        var createCommentCommand = Mapper.Map<CreateCommentCommand>(commendVm);
        createCommentCommand.UserId = UserId;
        var commentId = await Mediator.Send(createCommentCommand);

        return Created("api/comments", commentId);
    }
}