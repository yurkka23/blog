using FluentValidation;

namespace Blog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator() 
    {
        RuleFor(command => command.Message)
            .NotEmpty()
            .WithMessage("Massage must not be empty!");

        RuleFor(command => command.Message)
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
