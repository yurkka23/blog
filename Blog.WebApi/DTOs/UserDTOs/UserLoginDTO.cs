namespace Blog.WebApi.DTOs.UserDTOs;

public class UserLoginDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
