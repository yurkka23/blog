using Blog.Application.Articles.Queries.GetArticleList;
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQuery : IRequest<ArticleListVm>
{
    public Guid UserId { get; set; }
}
