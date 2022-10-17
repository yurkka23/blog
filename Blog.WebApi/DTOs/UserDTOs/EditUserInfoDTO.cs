namespace Blog.WebApi.DTOs.UserDTOs;

public class EditUserInfoDTO : IMapWith<EditUserInfoCommand>
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;
    public string ImageUserUrl { get; set; } = string.Empty;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<EditUserInfoDTO, EditUserInfoCommand>();
    }
}
