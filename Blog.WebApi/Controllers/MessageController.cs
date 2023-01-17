using Blog.Application.Messages.Commands.CreateMessage;
using Blog.Application.Messages.Commands.DeleteMessage;
using Blog.Application.Messages.Queries.GetMessagesFromGroup;
using Blog.Application.Messages.Queries.GetUserListOfChats;
using Blog.WebApi.DTOs.MessageDTOs;
using Microsoft.AspNetCore.Http;

namespace Blog.WebApi.Controllers;

[Route("message/")]
[Authorize]
[ApiController]
public class MessageController : BaseController
{
    private readonly IMapper _mapper;
    public MessageController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpPost("create-message")]
    public async Task<ActionResult<Guid>> CreateMessage([FromBody] CreateMessageDTO request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateMessageCommand>(request);
        command.SenderId = UserId;
        var messageId = await Mediator.Send(command, cancellationToken);

        return Ok(messageId);
    }

    [HttpDelete("delete-message")]
    public async Task<ActionResult> DeleteMessage(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteMessageCommand
        {
            MessageId = id,
            UserId = UserId,
        }, cancellationToken);

        return Ok();
    }

    [HttpGet("get-list-of-messages-from-group")]
    public async Task<ActionResult<MessagesList>> GetMessagesFromGroup(Guid recipientUserId, CancellationToken cancellationToken)
    {
        var query = new GetMessagesFromGroupQuery
        {
            CurrentUserId = UserId,
            RecipientUserId = recipientUserId
        };

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("get-user-list-of-chats")]
    public async Task<ActionResult<ListOfChats>> GetUserListOfChats(CancellationToken cancellationToken)
    {
        var query = new GetUserListOfChatsQuery
        {
            UserId = UserId
        };

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}
