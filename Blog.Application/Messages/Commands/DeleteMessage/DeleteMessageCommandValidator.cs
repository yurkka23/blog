using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    private readonly IMongoCollection<Message> _entitiesCollection;
    private readonly IMongoCollection<User> _userCollection;
    public DeleteMessageCommandValidator(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
          entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Message>(
            entitiesStoreDatabaseSettings.Value.CollectionName);

        var mongoClientUser = new MongoClient(
          userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        RuleFor(m => m.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .Must((id) => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such User doesn't exists in Users");

        RuleFor(m => m.MessageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("MessageId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("MessageId must not be empty")
            .Must((id) => _entitiesCollection.AsQueryable().Any(t => t.EntityId == id))
            .WithMessage("Such message doesn't exists in Messages");

    }
}