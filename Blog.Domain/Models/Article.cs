using Blog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Blog.Domain.Models;

public class Article : MongoEntity
{
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public State State { get; set; } = State.Waiting;
    public string Genre { get; set; } = string.Empty;
    public string ArticleImageUrl { get; set; } = string.Empty;


    //public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

   // public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

}
