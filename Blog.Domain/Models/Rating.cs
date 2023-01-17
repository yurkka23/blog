namespace Blog.Domain.Models;

public class Rating : MongoEntity
{
    public byte Score { get; set; }
    public Guid ArticleId { get; set; }
    public DateTime CreatedTime { get; set; }

}
