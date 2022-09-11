using System;
using FluentValidation;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQueryValidator: AbstractValidator<GetArticlesByUserQuery>
{
    public GetArticlesByUserQueryValidator()
    {
        RuleFor(article => article.UserId).NotEqual(Guid.Empty).WithMessage("Article must have user Id");
    }
}
