using Blog.Application.Users.Queries.GetUsersByRole;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Users.Queries.SearchUser;

public class SearchUserQuery : IRequest<UserList>
{
    public Role Role { get; set; }
    public string PartUsername { get; set; } 
}
