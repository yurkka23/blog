using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class UserInfoVm : IMapWith<User>
{
    //view model will return to client
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AboutMe { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfoVm>();
    }
}
