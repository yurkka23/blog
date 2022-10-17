using FluentValidation;


namespace Blog.Application.Users.Commands.ChangeRoleToAdmin;

public class ChangeRoleToAdminCommandValidator : AbstractValidator<ChangeRoleToAdminCommand>
{
    public ChangeRoleToAdminCommandValidator()
    {
        RuleFor(user => user.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("UserId must not be empty");     
    }
}
