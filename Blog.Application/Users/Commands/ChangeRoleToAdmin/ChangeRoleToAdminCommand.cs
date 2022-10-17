using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Users.Commands.ChangeRoleToAdmin;

public class ChangeRoleToAdminCommand : IRequest
{
    public Guid UserId { set; get; }
    public Role Role { set; get; }
}
