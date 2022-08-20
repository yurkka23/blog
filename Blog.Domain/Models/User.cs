using Blog.Domain.Enums;
using System;
using System.Collections.Generic;


namespace Blog.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
        public Role Role { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
