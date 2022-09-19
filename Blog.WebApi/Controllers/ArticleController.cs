using Microsoft.AspNetCore.Mvc;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Domain.Enums;
using Blog.Application.Articles.Queries.GetArticleContent;
using AutoMapper;
using Blog.Application.Articles.Queries.GetArticlesByUser;
using Blog.WebApi.DTOs.ArticeDTOs;
using Blog.Application.Articles.Commands.CreateArticle;
using Blog.Application.Articles.Commands.UpdateArticle;
using Blog.Application.Articles.Commands.VerifyArticle;
using Blog.Application.Articles.Commands.DeleteArticle;
using Blog.Persistence.Services;
using Microsoft.AspNetCore.Authorization;

namespace Blog.WebApi.Controllers;

[Route("article/")]
[ApiController]
public class ArticleController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ArticleController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;//
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
    public async Task<ActionResult<ArticleListVm>> GetUserArticles(Guid userId)
    {
        var query = new GetArticlesByUserQuery
        {
            UserId = userId
        };
        var vm = await Mediator.Send(query);
      
        return Ok(vm);
    }

  
    [HttpPost("CreateArticle")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateArticle([FromBody] CreateArticleDTO createArticleDto)
    {
        var command = _mapper.Map<CreateArticleCommand>(createArticleDto);
        //command.UserId = UserId;
        command.UserId = _userService.GetUserId(HttpContext);//
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
    public async Task<IActionResult> VerufyArticle([FromBody] VerifyArticleDTO verifyArticleDto)
    {
        var command = _mapper.Map<VerifyArticleCommand>(verifyArticleDto);
        command.UserId = UserId;
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

