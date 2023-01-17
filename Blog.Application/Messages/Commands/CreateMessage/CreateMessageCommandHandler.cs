using Blog.Domain.Models;
using MediatR;
using Blog.Application.Caching;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Common.Exceptions;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
{
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;

    public CreateMessageCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<Guid> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var sender = (await _userCollection
          .FindAsync(x => x.Id == request.SenderId,null, cancellationToken))
          .FirstOrDefault();

        var recipient = (await _userCollection
          .FindAsync(x => x.Id == request.RecipientId, null, cancellationToken))
          .FirstOrDefault();

        var message = new Message
        {
            EntityId = Guid.NewGuid(),
            UserId = sender.Id,
            RecipienId = recipient.Id,
            SenderId = sender.Id,
            Content = request.Content.Trim(),
            SenderUsername = sender.UserName,
            RecipienUsername = recipient.UserName,
            MessageSent = DateTime.UtcNow,
            DateRead = null,
            RecipientDeleted = false,
            SenderDeleted = false
        };

        await _entitiesCollection.InsertOneAsync(message, cancellationToken);

        return message.EntityId;
    }

}
