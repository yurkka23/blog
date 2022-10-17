using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Articles.Queries.SearchArticlesByTitle;

public class SearchArticlesByTitleQuery : IRequest<ArticleList>
{
    public State State { get; set; }
    public string PartTitle { get; set; } = string.Empty;   
}
