using MediatR;

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class GetUserListOfChatsQuery : IRequest<ListOfChats>
{
    public Guid UserId { get; set; }
}

