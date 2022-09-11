using System;
using FluentValidation;

namespace Blog.Application.Articles.Commands.VerifyArticle
{
    public class VerifyArticleCommandValidator : AbstractValidator<VerifyArticleCommand>
    {
        public VerifyArticleCommandValidator()
        {
            RuleFor(command => command.Id).NotEqual(Guid.Empty).WithMessage("Article must have Id");
            RuleFor(command => command.UserId).NotEqual(Guid.Empty).WithMessage("Article must have user Id");
        }
    }
}
