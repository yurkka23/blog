
using Blog.Application.Common.Helpers;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQuery : PaginationParams, IRequest<PagedList<ArticleByUserLookupDto>> 
{
    public Guid UserId { get; set; }
}
