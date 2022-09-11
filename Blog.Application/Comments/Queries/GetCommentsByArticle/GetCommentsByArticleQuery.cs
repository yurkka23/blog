using MediatR;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQuery : IRequest<CommentListVm>
{
    public Guid ArticleId { get; set; }
}
