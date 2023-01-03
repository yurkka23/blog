
using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscribedTo;

public class GetUserSubscribedToQuery : IRequest<UserList>
{
    public Guid UserId { get; set; }
}