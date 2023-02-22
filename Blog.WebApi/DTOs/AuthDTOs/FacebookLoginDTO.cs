namespace Blog.WebApi.DTOs.AuthDTOs;

public class FacebookLoginDTO
{
    public string Id { get; set; } = string.Empty;
    public string? Email { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

}

