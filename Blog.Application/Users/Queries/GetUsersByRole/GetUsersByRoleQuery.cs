using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Users.Queries.GetUsersByRole;

public class GetUsersByRoleQuery : IRequest<UserList>
{
    public Role Role { get; set; }
}

