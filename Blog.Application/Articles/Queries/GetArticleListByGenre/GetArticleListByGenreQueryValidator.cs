using FluentValidation;

namespace Blog.Application.Articles.Queries.GetArticleListByGenre;

public class GetArticleListByGenreQueryValidator : AbstractValidator<GetArticleListByGenreQuery>
{
    public GetArticleListByGenreQueryValidator()
    {
        RuleFor(article => article.Genre)
            .MaximumLength(15)
            .WithMessage("Article genre must not be longer 15");
    }
}
