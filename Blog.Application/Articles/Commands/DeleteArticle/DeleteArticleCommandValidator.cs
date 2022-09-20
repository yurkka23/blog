using FluentValidation;

namespace Blog.Application.Articles.Commands.DeleteArticle;

public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Article must have Id");

        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Article must have user Id");
    }
}
