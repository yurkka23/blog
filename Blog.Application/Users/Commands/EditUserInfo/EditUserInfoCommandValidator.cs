using System;
using FluentValidation;

namespace Blog.Application.Users.Commands.EditUserInfo
{
    public class EditUserInfoCommandValidator : AbstractValidator<EditUserInfoCommand>
    {
        public EditUserInfoCommandValidator()
        {
            RuleFor(user => user.Id)
                .NotEqual(Guid.Empty);

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("FirstName must not be empty")
                .MaximumLength(20).WithMessage("FirstName must not be longer then 20");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("LastName must not be empty")
                .MaximumLength(20).WithMessage("LastName must not be longer then 20");

            RuleFor(user => user.UserName).NotEmpty()
                .NotEmpty().WithMessage("UserName must not be empty")
                .MaximumLength(20).WithMessage("UserName must not be longer then 20");

            RuleFor(user => user.AboutMe)
                .NotEmpty().WithMessage("AboutMe must not be empty")
                .MaximumLength(20).WithMessage("AboutMe must not be longer then 600");
        }
    }
}
