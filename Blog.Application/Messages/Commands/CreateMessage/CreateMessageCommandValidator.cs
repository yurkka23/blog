using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator(IBlogDbContext dbContext)
    {
        RuleFor(m => m.RecipientId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RecipientId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("RecipientId must not be empty")
            .NotEqual(m => m.SenderId)
            .WithMessage("RecipientId must not be equal to SenderId")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such Recipient doesn't exists in Users");

        RuleFor(m => m.SenderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("SenderId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("SenderId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such Sender doesn't exists in Users");

        RuleFor(m => m.Content)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Content can't be empty")
            .MaximumLength(1000)
            .WithMessage("Content must not be londer 1000");
    }
}