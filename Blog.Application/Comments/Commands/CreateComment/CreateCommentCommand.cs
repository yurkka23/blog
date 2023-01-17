using MediatR;

namespace Blog.Application.Comments.Commands.CreateComment;
public class CreateCommentCommand : IRequest<Guid>
{
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}
