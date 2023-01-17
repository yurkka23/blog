using Blog.Domain.Models;
using MediatR;

namespace Blog.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<User>
{
    public string UserName { get; set; } 
}
