using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.DTOs.UserDTOs;

public class UserRegisterDTO
{
    [Required]

    public string UserName { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    public string? AboutMe { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}
