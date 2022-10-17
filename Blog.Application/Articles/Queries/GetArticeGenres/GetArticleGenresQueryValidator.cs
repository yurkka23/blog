using FluentValidation;

namespace Blog.Application.Articles.Queries.GetArticeGenres;

public class GetArticleGenresQueryValidator : AbstractValidator<GetArticleGenresQuery>
{
    public GetArticleGenresQueryValidator()
    {
        RuleFor(genre => genre.CountGenres)
            .GreaterThan(0)
            .WithMessage("CountGenres must have be bigger then 0");
    }

}
