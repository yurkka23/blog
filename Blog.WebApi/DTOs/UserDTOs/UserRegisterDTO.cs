namespace Blog.WebApi.DTOs.UserDTOs;

public class UserRegisterDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string? AboutMe { get; set; }
    [Required]
    //[DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

}
