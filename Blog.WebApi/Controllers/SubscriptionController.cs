using Blog.Application.UserSubscriptions.Commands.CreateSubscription;
using Blog.Application.UserSubscriptions.Commands.DeleteSubscription;
using Blog.Application.UserSubscriptions.Queries.GetUserSubscribedTo;
using Blog.Application.UserSubscriptions.Queries.GetUserSubscriptions;
using Blog.WebApi.DTOs.SubscriptionDTOs;

namespace Blog.WebApi.Controllers;

[Route("subscription/")]
[Authorize]
[ApiController]
public class SubscriptionController : BaseController
{
    private readonly IMapper _mapper;
    public SubscriptionController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create-subscription")]
    public async Task<ActionResult<Unit>> CreateSubscription([FromBody] CreateSubscriptionDTO request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSubscriptionCommand>(request);
        command.UserId = UserId;
        await Mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("delete-subscription")]
    public async Task<ActionResult<Unit>> DeleteSubscription(Guid userToSubscribeId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteSubscriptionCommand
        {
            UserToSubscribeId = userToSubscribeId,
            UserId = UserId
        }, cancellationToken);

        return Ok();
    }

    [HttpGet("user-subscribed-to")]
    public async Task<ActionResult<UserList>> UserSubscribedTo(Guid UserId, CancellationToken cancellationToken)
    {
        var query = new GetUserSubscribedToQuery
        {
            UserId = UserId
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("user-subscriptions")]
    public async Task<ActionResult<UserList>> UserSubscriptions(Guid UserId, CancellationToken cancellationToken)
    {
        var query = new GetUserSubscriptionQuery
        {
            UserId = UserId
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

}