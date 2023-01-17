
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Domain.Models;

public class Connection
{
    [BsonId]
    public Guid Id { get; set; }
    public string GroupName { get; set; }
    public string ConnectionId { get; set; }
    public string Username { get; set; }
}
