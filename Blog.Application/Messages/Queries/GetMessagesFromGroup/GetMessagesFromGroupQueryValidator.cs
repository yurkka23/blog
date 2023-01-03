using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class GetMessagesFromGroupQueryValidator : AbstractValidator<GetMessagesFromGroupQuery>
{
    public GetMessagesFromGroupQueryValidator(IBlogDbContext dbContext)
    {
        RuleFor(m => m.CurrentUserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("CurrentUserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("CurrentUserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such Current User doesn't exists in Users");

        RuleFor(m => m.RecipientUserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RecipientUserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("RecipientUserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such Recipient User doesn't exists in Messages");

    }
}