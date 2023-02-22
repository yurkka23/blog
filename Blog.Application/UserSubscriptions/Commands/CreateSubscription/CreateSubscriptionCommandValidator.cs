using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserSubscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator(IBlogDbContext dbContext)
    {
        RuleFor(user => user.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such user doesn't exists");

        RuleFor(user => user.UserToSubscribeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserToSubscribeId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such user doesn't exists");

        RuleFor(user => new { user.UserId, user.UserToSubscribeId })
            .MustAsync(async (id, cancellationToken) => await dbContext.UserSubscriptions.AllAsync(t => t.UserId != id.UserId || t.UserToSubscribeId != id.UserToSubscribeId, cancellationToken))
            .WithMessage("You already subscribed to this user");
    }
}
