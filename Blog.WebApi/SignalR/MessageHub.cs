using Blog.Application.Messages.Queries.GetMessagesFromGroup;
using Blog.WebApi.DTOs.MessageDTOs;
using Blog.WebApi.Extentions;

namespace Blog.WebApi.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMapper _mapper;
    private readonly IHubContext<PresenceHub> _presenceHub;
    private readonly IBlogDbContext _dbContext;
    private readonly IMediator _mediator;

    public MessageHub(IBlogDbContext dbContext, IMapper mapper, IHubContext<PresenceHub> presenceHub, IMediator mediator)
    {
        _dbContext = dbContext;
        _presenceHub = presenceHub;
        _mapper = mapper;
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUserId = httpContext.Request.Query["user"];
        var anotherUserEntity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(otherUserId.ToString()));

        var groupName = GetGroupName(Context.User.GetUsername(), anotherUserEntity.UserName);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        if(anotherUserEntity == null)
        {
            throw new HubException("Another User doesn't exists");
        }

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await _mediator.Send(new GetMessagesFromGroupQuery
        {
            CurrentUserId = Context.User.GetUserId(),
            RecipientUserId = anotherUserEntity.Id
        });

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDTO createMessageDto)
    {
        var username = Context.User.GetUsername();
        var userId = Context.User.GetUserId();

        if (userId == createMessageDto.RecipientId)
            throw new HubException("You cannot send messages to yourself");

        var sender = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var recipient = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == createMessageDto.RecipientId);

        if (recipient == null) throw new HubException("Not found recipient user");

        var message = new Message
        {
            RecipienId = recipient.Id,
            SenderId = sender.Id,
            Content = createMessageDto.Content.Trim(),
            SenderUsername = sender.UserName,
            RecipienUsername = recipient.UserName,
            Sender = sender,
            Recipient = recipient,
            MessageSent = DateTime.UtcNow
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);

        var group = await _dbContext.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        if (group.Connections.Any(x => x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if (connections != null)
            {
                await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", 
                    new { username = sender.UserName, userId = sender.Id, content = message.Content});
            }
        }

        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<Application.Messages.Queries.GetMessagesFromGroup.MessageDTO>(message));
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        try
        {
            var group = await _dbContext.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                await _dbContext.Groups.AddAsync(group);
            }

            group.Connections.Add(connection);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            return group;
        }
        catch(Exception ex)
        {
            throw new HubException("Failed to add to group");
        }
        
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        try
        {
            var group = await _dbContext.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections.Any(c => c.ConnectionId == Context.ConnectionId))
            .FirstOrDefaultAsync();

            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            _dbContext.Connections.Remove(connection);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return group;
        }
        catch(Exception ex)
        {
            throw new HubException("Failed to remove from group");

        }

    }
}
