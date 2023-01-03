using Blog.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscribedTo;

public class GetUserSubscribedToQueryValidator : AbstractValidator<GetUserSubscribedToQuery>
{
    public GetUserSubscribedToQueryValidator(IBlogDbContext dbContext)
    {
        RuleFor(user => user.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId can't be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty")
            .MustAsync(async (id, cancellationToken) => await dbContext.Users.AnyAsync(t => t.Id == id, cancellationToken))
            .WithMessage("Such user doesn't exists in Users");
    }
}
