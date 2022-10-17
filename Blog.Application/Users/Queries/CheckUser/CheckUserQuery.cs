using MediatR;

namespace Blog.Application.Users.Queries.CheckUser;

public class CheckUserQuery : IRequest<bool>
{
    public string UserName { get; set; } = string.Empty;
}
