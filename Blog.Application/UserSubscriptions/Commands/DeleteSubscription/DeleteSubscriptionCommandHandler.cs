using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MediatR;
using Blog.Domain.Models;

namespace Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand>
{
    private readonly IMongoCollection<UserSubscription> _entitiesCollection;

    public DeleteSubscriptionCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<UserSubscription>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<Unit> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await _entitiesCollection
            .DeleteOneAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") 
            & Builders<UserSubscription>.Filter.Eq("UserId", request.UserId) 
            & Builders<UserSubscription>.Filter.Eq("UserSubscribedToId", request.UserToSubscribeId), cancellationToken);

        return Unit.Value;
    }
}
