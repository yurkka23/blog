
namespace Blog.Domain.Models;

public class Message : MongoEntity
{
    public Guid SenderId { get; set; }
    public string SenderUsername {  get; set; }
    public Guid RecipienId { get; set; }
    public string RecipienUsername { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

}
