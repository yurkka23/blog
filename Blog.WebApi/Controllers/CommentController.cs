namespace Blog.WebApi.Controllers;

[Route("comment/")]
[ApiController]
public class CommentController : BaseController
{
    private readonly IMapper _mapper;

    public CommentController(IMapper mapper) => _mapper = mapper;

    [HttpGet("GetCommentsByArticle")]
    [Authorize]
    public async Task<ActionResult<CommentListVm>> GetCommentsByArticle(Guid articleId)
    {
        var query = new GetCommentsByArticleQuery
        {
            ArticleId = articleId
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost("CreateCommentToArticle")]
    [Authorize]
    public async Task<ActionResult<int>> CreateCommentToArticle([FromBody] CreateCommentDTO createCommentDto)
    {
        var command = _mapper.Map<CreateCommentCommand>(createCommentDto);
        command.UserId = UserId;
        var commnetId = await Mediator.Send(command);
        return Ok(commnetId);
    }

    [HttpPut("UpdateComment")]
    [Authorize]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDTO updateCommentDto)
    {
        var command = _mapper.Map<UpdateCommentCommand>(updateCommentDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("DeleteComment")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(int id)
    {

        var commentId = await Mediator.Send(new DeleteCommentCommand
        {
            Id = id,
            UserId = UserId
        });
        return NoContent();
    }
}
