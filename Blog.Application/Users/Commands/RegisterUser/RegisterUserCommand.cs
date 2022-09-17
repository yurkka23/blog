using MediatR;

namespace Blog.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
