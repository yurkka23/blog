using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using Blog.Domain.Models;

namespace Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandValidator : AbstractValidator<DeleteSubscriptionCommand>
{
    private readonly IMongoCollection<UserSubscription> _entitiesCollection;

    public DeleteSubscriptionCommandValidator(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings)
    {
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
            .MustAsync(async (id, cancellationToken) => (await _entitiesCollection
                 .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") & Builders<UserSubscription>.Filter.Eq("UserId", id)))
                 .FirstOrDefault() == null ? false : true)
            .WithMessage("Such user doesn't exists in User Subscriptions");

        RuleFor(user => user.UserToSubscribeId)
           .Cascade(CascadeMode.Stop)
           .NotEmpty()
           .WithMessage("UserId can't be empty")
           .NotEqual(Guid.Empty)
           .WithMessage("UserId must not be empty")
           .MustAsync(async (id, cancellationToken) => (await _entitiesCollection
                .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription") & Builders<UserSubscription>.Filter.Eq("UserSubscribedToId", id)))
                .FirstOrDefault() == null ? false : true)
           .WithMessage("Such user doesn't exists in User Subscriptions");

        RuleFor(user => new { user.UserId, user.UserToSubscribeId })
           .MustAsync(async (id, cancellationToken) => (await _entitiesCollection
                   .FindAsync(Builders<UserSubscription>.Filter.Eq("_t", "UserSubscription")))
                   .ToEnumerable()
                   .Any(t => t.UserId == id.UserId && t.UserSubscribedToId == id.UserToSubscribeId))
           .WithMessage("Such subscription doesn't exist in  User Subscriptions");

    }
}
