using Blog.Application.Common.Exceptions;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Blog.Application.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : AsyncRequestHandler<DeleteMessageCommand>
{
    private readonly IMongoCollection<Message> _messageCollection;
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;

    public DeleteMessageCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        _messageCollection = mongoDatabase.GetCollection<Message>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

    }
    protected override async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = (await _messageCollection.FindAsync(Builders<Message>.Filter.Eq("_t", "Message") & Builders<Message>.Filter.Eq("_id", request.MessageId), null, cancellationToken)).FirstOrDefault();

        if (message.SenderId == request.UserId) message.SenderDeleted = true;
        if (message.RecipienId == request.UserId) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted)
        {
            await _messageCollection.DeleteOneAsync(Builders<Message>.Filter.Eq("_t", "Message") & Builders<Message>.Filter.Eq("_id", request.MessageId), cancellationToken);
        }

        await _entitiesCollection.ReplaceOneAsync(x => x.EntityId == request.MessageId, message, new ReplaceOptions { IsUpsert = false }, cancellationToken);
    }
}
