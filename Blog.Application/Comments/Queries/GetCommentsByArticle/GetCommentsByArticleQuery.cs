using MediatR;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQuery : IRequest<CommentList>
{
    public Guid ArticleId { get; set; }
}
