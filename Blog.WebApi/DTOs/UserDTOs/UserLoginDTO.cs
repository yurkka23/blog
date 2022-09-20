namespace Blog.WebApi.DTOs.UserDTOs;

public class UserLoginDTO
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
