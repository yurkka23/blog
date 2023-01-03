using MediatR;

namespace Blog.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<Guid>
{
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public string Content { get; set; }
   
}
