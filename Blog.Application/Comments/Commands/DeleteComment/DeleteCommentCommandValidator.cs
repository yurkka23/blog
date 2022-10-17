using FluentValidation;

namespace Blog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .WithMessage("Comment Id must not be empty");
    }
}
