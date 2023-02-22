using Blog.WebApi.Extentions;

namespace Blog.WebApi.Controllers;

[Route("article/")]
[ApiController]
public class ArticleController : BaseController
{
    private readonly IMapper _mapper;
       
    public ArticleController(IMapper mapper, IMediator mediator): base(mediator) 
    {
        _mapper = mapper;
    }

    [HttpGet("get-list-of-articles")]
    public async Task<ActionResult<ArticleList>> GetAllArticles([FromQuery] GetArticleListQuery query, CancellationToken cancellationToken)
    {
        query.State = State.Approved;
        var response = await Mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
               response.TotalCount, response.TotalPages);

        return Ok(response);
    }

    [HttpGet("get-top-articles")]
    public async Task<ActionResult<ArticleList>> GetTopArticles(CancellationToken cancellationToken)
    {
        var query = new GetTopArticlesQuery
        {
            State = State.Approved
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    [HttpGet("get-article-genres")]
    public async Task<ActionResult<GenresList>> GetArticleGenres(int count , CancellationToken cancellationToken)
    {
        var query = new GetArticleGenresQuery
        {
            CountGenres = count
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    [HttpGet("get-articles-by-genres")]
    public async Task<ActionResult<ArticleList>> GetArticlesByGenres([FromQuery] GetArticleListByGenreQuery query, CancellationToken cancellationToken)
    {
        query.State = State.Approved;

        var response = await Mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);
        return Ok(response);
    }
    [HttpGet("search-articles-by-title")]
    public async Task<ActionResult<ArticleList>> SearchArticlesByTitle([FromQuery] SearchArticlesByTitleQuery query, CancellationToken cancellationToken)
    {
        query.State = State.Approved;

        var response = await Mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);

        return Ok(response);
    }

    [HttpGet("get-article-content-by-id")]
    public async Task<ActionResult<ArticleContent>> GetArticleContentById(Guid id, Guid CurrentUserId, CancellationToken cancellationToken)
    {
        var query = new GetArticleContentQuery
        {
            Id = id,
            UserId = CurrentUserId
        };
        var vm = await Mediator.Send(query, cancellationToken);
    
        return Ok(vm);
    }

    [HttpGet("get-user-articles")]
    [Authorize]
    public async Task<ActionResult<ArticleList>> GetUserArticles([FromQuery] GetArticlesByUserQuery query,CancellationToken cancellationToken)
    {
        query.UserId = UserId;

        var response = await Mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);

        return Ok(response);
    }

    [HttpGet("get-another-user-articles")]
    [Authorize]
    public async Task<ActionResult<ArticleList>> GetAnotherUserArticles([FromQuery] GetArticlesByUserQuery query, CancellationToken cancellationToken)
    {

        var response = await Mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);

        return Ok(response);
    }


    [HttpPost("create-article")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateArticle([FromBody] CreateArticleDTO createArticleDto,CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateArticleCommand>(createArticleDto);
        command.UserId = UserId;
        var articleId = await Mediator.Send(command, cancellationToken);
        return Ok(articleId);
    }

    [HttpPut("update-article")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleDTO updateArticleDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateArticleCommand>(updateArticleDto);
        command.UserId = UserId;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }


    [HttpDelete("delete-article")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(Guid id, CancellationToken cancellationToken)
    {

        var articleId = await Mediator.Send(new DeleteArticleCommand
        {
            Id = id,
            UserId = UserId,
            Role = UserRole == "User" ? Role.User : Role.Admin,
            
        }, cancellationToken);
        return NoContent();
    }

    [HttpPut("verify-article")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerufyArticle([FromBody] VerifyArticleDTO verifyArticleDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<VerifyArticleCommand>(verifyArticleDto);
        command.Role = UserRole == "User" ? Role.User : Role.Admin;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("get-list-of-waiting-articles")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleList>> GetWaitingArticles([FromQuery] GetArticleListQuery query, CancellationToken cancellationToken)
    {
        query.State = State.Waiting;

        var response = await Mediator.Send(query, cancellationToken);


        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);

        return Ok(response);
    }

    [HttpGet("search-waiting-articles-by-title")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ArticleList>> SearchWaitingArticlesByTitle([FromQuery] SearchArticlesByTitleQuery query, CancellationToken cancellationToken)
    {
        query.State = State.Waiting;

        var response = await Mediator.Send(query, cancellationToken);


        Response.AddPaginationHeader(response.CurrentPage, response.PageSize,
              response.TotalCount, response.TotalPages);

        return Ok(response);
    }
}

