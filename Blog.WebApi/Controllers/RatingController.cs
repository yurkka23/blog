namespace Blog.WebApi.Controllers;

[Route("rating/")]
[ApiController]
public class RatingController : BaseController
{
    private readonly IMapper _mapper;

    public RatingController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    } 

    [HttpGet("get-rating-list-by-article")]
    public async Task<ActionResult<RatingList>> GetRatingListByArticle(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRatingListByArticleQuery
        {
            ArticleId = id
        };
        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpGet("get-rating-list-by-user")]
    [Authorize]
    public async Task<ActionResult<RatingList>> GetRatingListByUser(CancellationToken cancellationToken)
    {
        var query = new GetRatingListByUserQuery
        {
            UserId = UserId
        };
        var vm = await Mediator.Send(query, cancellationToken);

        return Ok(vm);
    }

    [HttpPost("create-rating-to-article")]
    [Authorize]
    public async Task<ActionResult<int>> CreateRating([FromBody] CreateRatingDTO createRatingDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateRatingCommand>(createRatingDto);
        command.UserId = UserId;
        var ratingId = await Mediator.Send(command, cancellationToken);
        return Ok(ratingId);
    }

}
