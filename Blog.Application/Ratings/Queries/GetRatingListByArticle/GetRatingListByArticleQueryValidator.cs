using Blog.Application.Ratings.Queries.GetRatingByArticle;
using FluentValidation;

namespace Blog.Application.Ratings.Queries.GetRatingListByArticle;
public class GetRatingListByArticleQueryValidator : AbstractValidator<GetRatingListByArticleQuery>
{
    public GetRatingListByArticleQueryValidator()
    {
        RuleFor(c => c.ArticleId)
            .NotEqual(Guid.Empty)
            .WithMessage("Article Id must not be empty");
    }
}
