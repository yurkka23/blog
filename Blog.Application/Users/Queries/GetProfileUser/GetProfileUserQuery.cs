using MediatR;

namespace Blog.Application.Users.Queries.GetProfileUser;

public class GetProfileUserQuery : IRequest<ProfileUser>
{
    public Guid CurrentUserId { get; set; }
    public Guid Id { get; set; }
}
