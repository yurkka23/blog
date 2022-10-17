namespace Blog.WebApi.DTOs.AuthDTOs;

public class AuthResponseDTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? AboutMe { get; set; }
        public Role Role { get; set; }
        public string ImageUserUrl { get; set; } = string.Empty;
    }

    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserResponse User { get; set; }
  
}

