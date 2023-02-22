using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Helpers;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQuery : PaginationParams, IRequest<PagedList<ArticleLookupDto>>
{ 
    public State State { get; set; }
    public string PartTitle { get; set; } = string.Empty;   
}
