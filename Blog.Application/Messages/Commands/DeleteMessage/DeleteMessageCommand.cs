using MediatR;


namespace Blog.Application.Messages.Commands.DeleteMessage;

public class DeleteMessageCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid MessageId { get; set; }
}
