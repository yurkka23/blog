using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    private readonly IMongoCollection<User> _userCollection;

    public CreateMessageCommandValidator(IOptions<MongoUserDBSettings> userStoreDatabaseSettings)
    {
        var mongoClientUser = new MongoClient(
         userStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabaseUser = mongoClientUser.GetDatabase(
            userStoreDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabaseUser.GetCollection<User>(
            userStoreDatabaseSettings.Value.CollectionName);

        RuleFor(m => m.RecipientId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RecipientId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("RecipientId must not be empty")
            .NotEqual(m => m.SenderId)
            .WithMessage("RecipientId must not be equal to SenderId")
            .Must( (id) => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such Recipient doesn't exists in Users");

        RuleFor(m => m.SenderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("SenderId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("SenderId must not be empty")
            .Must((id) => _userCollection.AsQueryable().Any(t => t.Id == id))
            .WithMessage("Such Sender doesn't exists in Users");

        RuleFor(m => m.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Content can't be empty")
            .MaximumLength(1000)
            .WithMessage("Content must not be londer 1000");
    }
}