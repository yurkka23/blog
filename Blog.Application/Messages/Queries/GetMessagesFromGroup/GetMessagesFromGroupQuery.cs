using MediatR;
namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class GetMessagesFromGroupQuery : IRequest<MessagesList>
{
    public Guid CurrentUserId { get; set; }
    public Guid RecipientUserId { get; set; }
}

