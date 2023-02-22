using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Messages.Commands.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator(IBlogDbContext dbContext)
    {
        RuleFor(m => m.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such User doesn't exists in Users");

        RuleFor(m => m.MessageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("MessageId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("MessageId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Messages.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such message doesn't exists in Messages");

    }
}