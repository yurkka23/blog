using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class GetUserListOfChatsQueryValidator : AbstractValidator<GetUserListOfChatsQuery>
{
    public GetUserListOfChatsQueryValidator(IBlogDbContext dbContext)
    {
        RuleFor(m => m.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such  User doesn't exists in Users");

    }
}
