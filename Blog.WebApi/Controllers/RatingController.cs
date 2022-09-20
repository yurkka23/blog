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
    public async Task<ActionResult<RatingListVm>> GetRatingListByUser()///Guid id
    {
        var query = new GetRatingListByUserQuery
        {
            UserId = UserId
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
