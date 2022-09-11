using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public Role Role { get; set; }
}
