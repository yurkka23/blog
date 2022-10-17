namespace Blog.WebApi.DTOs.AuthDTOs;

public class AuthRequestDTO
{
    public Guid Id { get; set; }
    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
