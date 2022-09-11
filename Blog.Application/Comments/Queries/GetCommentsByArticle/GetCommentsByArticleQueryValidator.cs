using FluentValidation;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQueryValidator : AbstractValidator<GetCommentsByArticleQuery>
{
    public GetCommentsByArticleQueryValidator()
    {
        RuleFor(c => c.ArticleId)
            .NotEqual(Guid.Empty)
            .WithMessage("Comment must have Article Id");
    }
}
