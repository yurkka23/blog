namespace Blog.WebApi.DTOs.AuthDTOs;

public class AuthRefreshDTO
{
    public string jwtToken { get; set; } = string.Empty;
    public string refreshToken { get; set; } = string.Empty;
}
