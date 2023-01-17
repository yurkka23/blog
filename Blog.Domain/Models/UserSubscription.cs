namespace Blog.Domain.Models;

public class UserSubscription : MongoEntity
{
    public Guid UserSubscribedToId { get; set; }
}
