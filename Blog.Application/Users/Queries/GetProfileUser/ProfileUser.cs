using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetProfileUser;

public class ProfileUser : IMapWith<User>
{
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public int CountArticles { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public bool IsCurrentUserSubscribed { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, ProfileUser>()
                   .ForMember(art => art.AvatarUrl, art => art.MapFrom(map => map.ImageUserUrl));

    }
}
 