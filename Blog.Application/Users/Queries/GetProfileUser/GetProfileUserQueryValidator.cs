using FluentValidation;

namespace Blog.Application.Users.Queries.GetProfileUser;

public class GetProfileUserQueryValidator : AbstractValidator<GetProfileUserQuery>
{
    public GetProfileUserQueryValidator()
    {
        RuleFor(user => user.Id)
             .NotEqual(Guid.Empty)
             .WithMessage("User id must not be empty");
    }
}
