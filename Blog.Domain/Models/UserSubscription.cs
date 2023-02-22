namespace Blog.Domain.Models;

public class UserSubscription
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid UserToSubscribeId { get; set; }
    public User UserToSubscribe { get; set; }
}
