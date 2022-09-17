using Blog.Domain.Enums;

namespace Blog.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;//register and login by this  
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AboutMe { get; set; }
    public Role Role { get; set; }

    //Auth
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

    //Relations
    public ICollection<Article>? Articles { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public ICollection<Comment>? Comments { get; set; }

}
