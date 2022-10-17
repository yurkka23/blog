using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class UserInfo : IMapWith<User>
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;
    public string ImageUserUrl { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfo>();
    }
}
