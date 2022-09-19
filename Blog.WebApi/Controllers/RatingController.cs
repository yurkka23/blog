using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Blog.Application.Ratings.Queries;
using Blog.Application.Ratings.Queries.GetRatingByArticle;
using Blog.Application.Ratings.Queries.GetRatingListByUser;
using Blog.WebApi.DTOs.RatingDTOs;
using Blog.Application.Ratings.Commands.CreateRating;
using Microsoft.AspNetCore.Authorization;

namespace Blog.WebApi.Controllers;

[Route("rating/")]
[ApiController]
public class RatingController : BaseController
{
    private readonly IMapper _mapper;

    public RatingController(IMapper mapper) => _mapper = mapper;

    [HttpGet("GetRatingListByArticle")]
    public async Task<ActionResult<RatingListVm>> GetRatingListByArticle(Guid id)
    {
        var query = new GetRatingListByArticleQuery
        {
            ArticleId = id
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("GetRatingListByUser")]
    [Authorize]
    public async Task<ActionResult<RatingListVm>> GetRatingListByUser(Guid id)
    {
        var query = new GetRatingListByUserQuery
        {
            UserId = id
        };
        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpPost("CreateRatingToArticle")]
    [Authorize]
    public async Task<ActionResult<int>> CreateRating([FromBody] CreateRatingDTO createRatingDto)
    {
        var command = _mapper.Map<CreateRatingCommand>(createRatingDto);
        command.UserId = UserId;
        var ratingId = await Mediator.Send(command);
        return Ok(ratingId);
    }

}
