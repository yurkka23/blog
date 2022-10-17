namespace Blog.Domain.Models;

public class Rating
{
    public int Id { get; set; }
    public byte Score { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } 
}
