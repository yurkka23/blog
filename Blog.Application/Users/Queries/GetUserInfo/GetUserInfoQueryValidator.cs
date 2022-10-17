using FluentValidation;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
       RuleFor(user => user.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("User id must not be empty");
    }
}
