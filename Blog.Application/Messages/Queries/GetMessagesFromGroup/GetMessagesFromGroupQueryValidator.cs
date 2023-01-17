using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class GetMessagesFromGroupQueryValidator : AbstractValidator<GetMessagesFromGroupQuery>
{
    private readonly IMongoCollection<User> _userCollection;

    public GetMessagesFromGroupQueryValidator(IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        RuleFor(m => m.CurrentUserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("CurrentUserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("CurrentUserId must not be empty")
            .Must(id => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such Current User doesn't exists in Users");

        RuleFor(m => m.RecipientUserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RecipientUserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("RecipientUserId must not be empty")
            .Must(id => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such Recipient User doesn't exists in Messages");

    }
}