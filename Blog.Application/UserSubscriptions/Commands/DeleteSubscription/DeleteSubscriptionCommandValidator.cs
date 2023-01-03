using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserSubscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandValidator : AbstractValidator<DeleteSubscriptionCommand>
{
    public DeleteSubscriptionCommandValidator(IBlogDbContext dbContext)
    {
        RuleFor(user => user.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.UserSubscriptions.AnyAsync(t => t.UserId == id, cancellationToken))
            .WithMessage("Such user doesn't exists in User Subscriptions");

        RuleFor(user => user.UserToSubscribeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserToSubscribeId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.UserSubscriptions.AnyAsync(t => t.UserToSubscribeId == id, cancellationToken))
            .WithMessage("Such user doesn't exists in User Subscriptions");

        RuleFor(user => new { user.UserId, user.UserToSubscribeId })
            .MustAsync(async (id, cancellationToken) => await dbContext.UserSubscriptions.AnyAsync(t => t.UserId == id.UserId && t.UserToSubscribeId == id.UserToSubscribeId, cancellationToken))
            .WithMessage("Such subscription doesn't exist in  User Subscriptions");
    }
}
