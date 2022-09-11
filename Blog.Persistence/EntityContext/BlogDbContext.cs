using Microsoft.EntityFrameworkCore;
using Blog.Domain.Models;
using Blog.Application.Interfaces;
using Blog.Persistence.ModelsConfigurations;
using System;

namespace Blog.Persistence.EntityContext
{
    public class BlogDbContext : DbContext , IBlogDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options) 
        {
            Database.EnsureCreated();
         //   Database.Migrate();//to update-migration 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ArticleConfiguration());
            builder.ApplyConfiguration(new RatingConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            base.OnModelCreating(builder);
        }
        
    }
}
