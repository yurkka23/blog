using MediatR;

namespace Blog.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}
