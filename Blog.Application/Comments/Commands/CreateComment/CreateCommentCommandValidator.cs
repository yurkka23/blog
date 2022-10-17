using FluentValidation;

namespace Blog.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(command => command.Message)
            .NotEmpty()
            .WithMessage("Massage must not be empty!")
            .MaximumLength(200)
            .WithMessage("Massage must not be longer then 200!");

       

        RuleFor(command => command.ArticleId)
            .NotEqual(Guid.Empty)
            .WithMessage("Massage must have Article Id!");

        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Massage must have User Id!");
    }
}
