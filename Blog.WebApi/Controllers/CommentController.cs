namespace Blog.WebApi.Controllers;

[Route("comment/")]
[Authorize]
[ApiController]
public class CommentController : BaseController
{
    private readonly IMapper _mapper;

    public CommentController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpGet("get-comments-by-article")]
    [AllowAnonymous]
    public async Task<ActionResult<CommentList>> GetCommentsByArticle(Guid articleId, CancellationToken cancellationToken)
    {
        var query = new GetCommentsByArticleQuery
        {
            ArticleId = articleId
        };
        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpPost("create-comment-to-article")]
    public async Task<ActionResult<int>> CreateCommentToArticle([FromBody] CreateCommentDTO createCommentDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCommentCommand>(createCommentDto);
        command.UserId = UserId;
        var commnetId = await Mediator.Send(command, cancellationToken);
        return Ok(commnetId);
    }

    [HttpPut("update-comment")]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDTO updateCommentDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateCommentCommand>(updateCommentDto);
        command.UserId = UserId;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete-comment")]
    public async Task<IActionResult> DeleteComment(Guid id, CancellationToken cancellationToken)
    {

        var commentId = await Mediator.Send(new DeleteCommentCommand
        {
            Id = id,
            UserId = UserId,
            Role = UserRole == "User" ? Role.User : Role.Admin
        }, cancellationToken);
        return NoContent();
    }
}
