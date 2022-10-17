using MediatR;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQuery : IRequest<UserInfo>
{
    public Guid Id { get; set; }
}
