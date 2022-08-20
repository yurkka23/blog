using Blog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Blog.Domain.Models
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public State State { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
