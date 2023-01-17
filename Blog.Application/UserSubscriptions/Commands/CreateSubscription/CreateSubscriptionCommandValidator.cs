using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using Blog.Domain.Models;

namespace Blog.Application.UserSubscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    private readonly IMongoCollection<UserSubscription> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;

    public CreateSubscriptionCommandValidator(IOptions<MongoUserDBSettings> userStoreDatabaseSettings, IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
        var mongoClientUser = new MongoClient(
         userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<UserSubscription>(
            entitiesStoreDatabaseSettings.Value.CollectionName);


        RuleFor(user => user.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .Must( (id) =>  _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such user doesn't exists");

        RuleFor(user => user.UserToSubscribeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserToSubscribeId must not be empty")
            .Must((id) => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such user doesn't exists");

        RuleFor(user => new { user.UserId, user.UserToSubscribeId })
            .MustAsync(async (id, cancellationToken) => (await _entitiesCollection
                    .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription")))
                    .ToEnumerable()
                    .All(t => t.UserId != id.UserId || t.UserSubscribedToId != id.UserToSubscribeId))
            .WithMessage("You already subscribed to this user");
    }
}
