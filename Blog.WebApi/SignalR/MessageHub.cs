using Blog.Application.Messages.Queries.GetMessagesFromGroup;
using Blog.WebApi.DTOs.MessageDTOs;
using Blog.WebApi.Extentions;
using Microsoft.Extensions.Options;

namespace Blog.WebApi.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    private readonly IMongoCollection<Connection> _connectionsCollection;
    private readonly IMapper _mapper;
    private readonly IHubContext<PresenceHub> _presenceHub;
    private readonly IMediator _mediator;

    public MessageHub(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings,IOptions<MongoConnectionsDBSettings> connectionsStoreDatabaseSettings, IMapper mapper, IHubContext<PresenceHub> presenceHub, IMediator mediator)
    {
        _presenceHub = presenceHub;
        _mapper = mapper;
        _mediator = mediator;
        var mongoClient = new MongoClient(
           userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        var mongoClient1 = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase1 = mongoClient1.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase1.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClient2 = new MongoClient(
           connectionsStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase2 = mongoClient2.GetDatabase(
            connectionsStoreDatabaseSettings.Value.DatabaseName);

        _connectionsCollection = mongoDatabase2.GetCollection<Connection>(
            connectionsStoreDatabaseSettings.Value.CollectionName);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUserId = httpContext.Request.Query["user"];
        var anotherUserEntity = (await  _userCollection.FindAsync(x => x.Id == Guid.Parse(otherUserId.ToString()))).FirstOrDefault();

        var groupName = GetGroupName(Context.User.GetUsername(), anotherUserEntity.UserName);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        if (anotherUserEntity == null)
        {
            throw new HubException("Another User doesn't exists");
        }

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await _mediator.Send(new GetMessagesFromGroupQuery
        {
            CurrentUserId = Context.User.GetUserId(),
            RecipientUserId = anotherUserEntity.Id
        });

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group).SendAsync("UpdatedGroup");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDTO createMessageDto)
    {
        var username = Context.User.GetUsername();
        var userId = Context.User.GetUserId();

        if (userId == createMessageDto.RecipientId)
            throw new HubException("You cannot send messages to yourself");

        var sender = (await _userCollection.FindAsync(x => x.Id == userId)).FirstOrDefault();
        var recipient = (await _userCollection.FindAsync(x => x.Id == createMessageDto.RecipientId)).FirstOrDefault();

        if (recipient == null) throw new HubException("Not found recipient user");

        var message = new Message
        {
            EntityId = Guid.NewGuid(),
            RecipienId = recipient.Id,
            SenderId = sender.Id,
            Content = createMessageDto.Content.Trim(),
            SenderUsername = sender.UserName,
            RecipienUsername = recipient.UserName,
            MessageSent = DateTime.UtcNow
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);

        if (_connectionsCollection.AsQueryable().Any(x => x.Username == recipient.UserName && x.GroupName == groupName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if (connections != null)
            {
                await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, userId = sender.Id, content = message.Content });
            }
        }

        await _entitiesCollection.InsertOneAsync(message, CancellationToken.None);

        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<Application.Messages.Queries.GetMessagesFromGroup.MessageDTO>(message));
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private async Task<string> AddToGroup(string groupName)
    {
        try
        {
            var connection = new Connection
            {
                Id = Guid.NewGuid(),
                GroupName = groupName,
                ConnectionId = Context.ConnectionId,
                Username = Context.User.GetUsername()
            };

            await _connectionsCollection.InsertOneAsync(connection, CancellationToken.None);
           
            return connection.GroupName;
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to add to group");
        }

    }

    private async Task<string> RemoveFromMessageGroup()
    {
        try
        {
         
            var group = await _connectionsCollection.Find(x => x.ConnectionId == Context.ConnectionId).FirstOrDefaultAsync();
            await _connectionsCollection.DeleteOneAsync(x => x.ConnectionId == Context.ConnectionId);

            return group.GroupName;
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to remove from group");
        }

    }
}
