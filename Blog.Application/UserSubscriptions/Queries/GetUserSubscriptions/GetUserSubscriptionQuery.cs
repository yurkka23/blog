using Blog.Application.Users.Queries.GetUsersByRole;
using MediatR;

namespace Blog.Application.UserSubscriptions.Queries.GetUserSubscriptions;

public class GetUserSubscriptionQuery : IRequest<UserList>
{
    public Guid UserId { get; set; }
}