using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticleListByGenre;

public class GetArticleListByGenreQuery : IRequest<ArticleList>
{
    public State State { get; set; }
    public string Genre { get; set; } = string.Empty;
}