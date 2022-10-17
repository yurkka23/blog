using FluentValidation;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQueryValidator : AbstractValidator<SearchArticlesByTitleQuery>
{
    public SearchArticlesByTitleQueryValidator()
    {
        RuleFor(art => art.PartTitle)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("PartTitle must not be longer then 50");
        
    }
}
