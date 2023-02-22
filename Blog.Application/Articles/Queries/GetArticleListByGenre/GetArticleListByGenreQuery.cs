using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Helpers;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticleListByGenre;

public class GetArticleListByGenreQuery : PaginationParams, IRequest<PagedList<ArticleLookupDto>>
{
    public State State { get; set; }
    public string Genre { get; set; } = string.Empty;
}