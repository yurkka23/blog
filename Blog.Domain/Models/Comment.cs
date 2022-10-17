namespace Blog.Domain.Models;

public class Comment 
{
    public int Id { get; set; } 
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } 
}
