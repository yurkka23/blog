using MediatR;

namespace Blog.Application.Comments.Commands.CreateComment;
public class CreateCommentCommand : IRequest<int>
{
    public string Message { get; set; } = null!;

    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}
