using System;

namespace Blog.Domain.Models;

public class Comment : MongoEntity
{
    public string Message { get; set; } = string.Empty;
    public Guid ArticleId { get; set; }
    public DateTime CreatedTime { get; set; }
}
