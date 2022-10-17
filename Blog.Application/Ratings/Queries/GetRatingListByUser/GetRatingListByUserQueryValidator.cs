using FluentValidation;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;
public class GetRatingListByUserQueryValidator : AbstractValidator<GetRatingListByUserQuery>
{
    public GetRatingListByUserQueryValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("User Id must not be empty");
    }
}

