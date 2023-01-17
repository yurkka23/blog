using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class GetUserListOfChatsQueryHandler : IRequestHandler<GetUserListOfChatsQuery, ListOfChats>
{
    private readonly IMongoCollection<Message> _messagesCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMapper _mapper;

    public GetUserListOfChatsQueryHandler(IMapper mapper, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        _mapper = mapper;

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _messagesCollection = mongoDatabase.GetCollection<Message>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<ListOfChats> Handle(GetUserListOfChatsQuery request, CancellationToken cancellationToken)
    {
        var listUserId = (await _messagesCollection
            .FindAsync(Builders<Message>.Filter.Eq("_t", "Message") 
            & (Builders<Message>.Filter.Eq("RecipienId", request.UserId) & Builders<Message>.Filter.Eq("RecipientDeleted", false) 
            | Builders<Message>.Filter.Eq("SenderId", request.UserId) & Builders<Message>.Filter.Eq("SenderDeleted", false)), null, cancellationToken))
            .ToEnumerable()
            .OrderByDescending(m => m.MessageSent)
            .Select(c => c.RecipienId == request.UserId ? c.SenderId : c.RecipienId)
            .Distinct()
            .ToList();

        var chats = _userCollection.AsQueryable()
            .Where(u => listUserId.Contains(u.Id))
            .Select(u => new ChatDTO
            {
                RecipientUserId = u.Id,
                RecipientAvatarUrl = u.ImageUserUrl,
                RecipientUsername = u.UserName
            })
            .ToList();

        return new ListOfChats { Chats = chats };
    }

}
