using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscriptions;

public class GetUserSubscriptionQueryValidator : AbstractValidator<GetUserSubscriptionQuery>
{
    private readonly IMongoCollection<User> _userCollection;

    public GetUserSubscriptionQueryValidator(IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClientUser = new MongoClient(
        userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        RuleFor(user => user.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .Must(id => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such user doesn't exists in Users");
    }
}
