using Blog.Domain.Models;
using MediatR;
using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Application.UserSubscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand>
{
    private readonly IMongoCollection<MongoEntity> _entitiesCollection;

    public CreateSubscriptionCommandHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<MongoEntity>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<Unit> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new UserSubscription
        {
            UserId = request.UserId,
            UserSubscribedToId = request.UserToSubscribeId,
            EntityId = Guid.NewGuid()
        };

        await _entitiesCollection.InsertOneAsync(subscription, cancellationToken);

        return Unit.Value;
    }
}
