using Blog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Blog.Domain.Models;

public class Article : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public State State { get; set; } = State.Waiting;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } 

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

}
