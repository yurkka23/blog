using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AboutMe { get; set; }
    public Role Role { get; set; }

    //Auth
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpires { get; set; }

    //Relations
    public ICollection<Article>? Articles { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public ICollection<Comment>? Comments { get; set; }

}
