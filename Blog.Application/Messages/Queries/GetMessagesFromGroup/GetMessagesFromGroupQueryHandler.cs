using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class GetMessagesFromGroupQueryHandler : IRequestHandler<GetMessagesFromGroupQuery, MessagesList>
{
    private readonly IMongoCollection<Message> _messagesCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;

    private readonly IMongoCollection<User> _userCollection;
    private readonly IMapper _mapper;


    public GetMessagesFromGroupQueryHandler(IMapper mapper,IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
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

    public async Task<MessagesList> Handle(GetMessagesFromGroupQuery request, CancellationToken cancellationToken)
    {
        var messages = (await _messagesCollection
            .FindAsync(Builders<Message>.Filter.Eq("_t", "Message")
            & (Builders<Message>.Filter.Eq("RecipienId", request.CurrentUserId) & Builders<Message>.Filter.Eq("RecipientDeleted", false) & Builders<Message>.Filter.Eq("SenderId", request.RecipientUserId)
            | Builders<Message>.Filter.Eq("RecipienId", request.RecipientUserId) & Builders<Message>.Filter.Eq("SenderId", request.CurrentUserId) & Builders<Message>.Filter.Eq("SenderDeleted", false)), null, cancellationToken))
            .ToList()
            .OrderBy(m => m.MessageSent);

        var unreadMessages = messages.Where(m => m.DateRead == null
                && m.RecipienId == request.CurrentUserId).ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
                 _entitiesCollection.ReplaceOne(x => x.EntityId == message.EntityId, message, new ReplaceOptions { IsUpsert = false });
            }
        }

        var result = messages.Select(m => new MessageDTO
        {
            Id = m.EntityId,
            Content = m.Content,
            DateRead = m.DateRead,
            SenderId = m.SenderId,
            SenderUsername = m.SenderUsername,
            SenderPhotoUrl = _userCollection.Find(x => x.Id == m.SenderId).FirstOrDefault().ImageUserUrl,
            RecipienId = m.RecipienId,
            RecipienUsername = m.RecipienUsername,
            RecipientPhotoUrl = _userCollection.Find(x => x.Id == m.RecipienId).FirstOrDefault().ImageUserUrl,
            MessageSent = m.MessageSent,
        }).ToList();

        return new MessagesList { Messages = result };
    }

}

