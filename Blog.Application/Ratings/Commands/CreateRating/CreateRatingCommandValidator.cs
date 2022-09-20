using FluentValidation;

namespace Blog.Application.Ratings.Commands.CreateRating;

public class CreateRatingCommandValidator: AbstractValidator<CreateRatingCommand>
{
    public CreateRatingCommandValidator()
    {
        RuleFor(c => c.Score)
            .InclusiveBetween<CreateRatingCommand, byte>(1, 5)
            .WithMessage("Score must be between 1 - 5");

        RuleFor(c => c.ArticleId)
            .NotEqual(Guid.Empty)
            .WithMessage("Article Id must not be empty");
    }
}
