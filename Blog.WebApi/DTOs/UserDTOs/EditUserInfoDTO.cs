namespace Blog.WebApi.DTOs.UserDTOs;

public class EditUserInfoDTO : IMapWith<EditUserInfoCommand>
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string AboutMe { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EditUserInfoDTO, EditUserInfoCommand>();
    }
}
