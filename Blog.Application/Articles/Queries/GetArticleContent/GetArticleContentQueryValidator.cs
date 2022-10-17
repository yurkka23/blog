using FluentValidation;

namespace Blog.Application.Articles.Queries.GetArticleContent;

public class GetArticleContentQueryValidator : AbstractValidator<GetArticleContentQuery>
{
    public GetArticleContentQueryValidator()
    {
        RuleFor(article => article.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Article must have Id");
    }
}
