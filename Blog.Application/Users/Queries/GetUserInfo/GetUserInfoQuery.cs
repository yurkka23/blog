using MediatR;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQuery : IRequest<UserInfoVm>
{
    public Guid Id { get; set; }
}
