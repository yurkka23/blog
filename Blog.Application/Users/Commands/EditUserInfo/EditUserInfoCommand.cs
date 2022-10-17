using MediatR;

namespace Blog.Application.Users.Commands.EditUserInfo;

public class EditUserInfoCommand : IRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;
    public string ImageUserUrl { get; set; } = string.Empty;
}
