using FluentValidation;

namespace Blog.Application.Articles.Commands.CreateArticle;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(50);
           // .WithMessage("Title must not be longer then 50");
        RuleFor(command => command.Content)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Content must not be longer then 1000");
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Article must have user Id");
    }
}
