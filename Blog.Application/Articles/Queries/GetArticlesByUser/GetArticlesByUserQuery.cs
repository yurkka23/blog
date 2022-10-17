
using MediatR;

namespace Blog.Application.Articles.Queries.GetArticlesByUser;

public class GetArticlesByUserQuery : IRequest<ArticleListByUser>
{
    public Guid UserId { get; set; }
}
