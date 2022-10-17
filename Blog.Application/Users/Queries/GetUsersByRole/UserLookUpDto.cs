

using AutoMapper;
using Blog.Application.Common.Mappings;
using Blog.Domain.Enums;
using Blog.Domain.Models;

namespace Blog.Application.Users.Queries.GetUsersByRole;

public class UserLookUpDto : IMapWith<User>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AboutMe { get; set; }
    public Role Role { get; set; }
    public string ImageUserUrl { get; set; } = string.Empty;
   
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserLookUpDto>();
    }
}
