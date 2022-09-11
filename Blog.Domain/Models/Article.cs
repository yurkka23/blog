using Blog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Blog.Domain.Models
{
    public class Article : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public State State { get; set; } = State.Waiting;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Rating>? Ratings { get; set; } 

        public ICollection<Comment>? Comments { get; set; }

        public double? AverageRating => Ratings != null ? (Ratings.Count > 0 ? Ratings.Average(r => r.Score) : 0) : null;
    }
}
