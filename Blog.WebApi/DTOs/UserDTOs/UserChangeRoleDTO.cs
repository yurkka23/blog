using Blog.Application.Users.Commands.ChangeRoleToAdmin;

namespace Blog.WebApi.DTOs.UserDTOs;

public class UserChangeRoleDTO : IMapWith<ChangeRoleToAdminCommand>
{
    public Guid UserId { set; get; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserChangeRoleDTO, ChangeRoleToAdminCommand>();
    }
}