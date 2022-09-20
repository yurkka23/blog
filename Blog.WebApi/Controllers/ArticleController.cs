namespace Blog.WebApi.Controllers;

[Route("article/")]
[ApiController]
public class ArticleController : BaseController
{
    private readonly IMapper _mapper;

    public ArticleController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("GetListOfArticles")]
    public async Task<ActionResult<ArticleListVm>> GetAllArticles()
    {
        var query = new GetArticleListQuery
        {
           State = State.Approved
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("GetArticleContentById")]
    [Authorize]
    public async Task<ActionResult<ArticleContentVm>> GetArticleContentById(Guid id)
    {
        var query = new GetArticleContentQuery
        {
            Id = id
        };
        var vm = await Mediator.Send(query);
    
        return Ok(vm);
    }

    [HttpGet("GetUserArticles")]
    [Authorize]
    public async Task<ActionResult<ArticleListVm>> GetUserArticles()//Guid userId
    {
        var query = new GetArticlesByUserQuery
        {
            UserId = UserId//
        };
        var vm = await Mediator.Send(query);
      
        return Ok(vm);
    }

  
    [HttpPost("CreateArticle")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateArticle([FromBody] CreateArticleDTO createArticleDto)
    {
        var command = _mapper.Map<CreateArticleCommand>(createArticleDto);
        command.UserId = UserId;
        //command.UserId = _userService.GetUserId(HttpContext);//
        var articleId = await Mediator.Send(command);
        return Ok(articleId);
    }

    [HttpPut("UpdateArticle")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleDTO updateArticleDto)
    {
        var command = _mapper.Map<UpdateArticleCommand>(updateArticleDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPut("VerifyArticle")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerufyArticle([FromBody] VerifyArticleDTO verifyArticleDto)//without role 
    {
        var command = _mapper.Map<VerifyArticleCommand>(verifyArticleDto);
        command.Role = UserRole == "User" ? Role.User : Role.Admin;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("DeleteArticle")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {

        var articleId = await Mediator.Send(new DeleteArticleCommand
        {
            Id = id,
            UserId = UserId
        });
        return NoContent();
    }
}

