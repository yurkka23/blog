using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.GetTopArticles;

public class GetTopArticlesQuery : IRequest<ArticleList>
{
    public State State { get; set; }
}
