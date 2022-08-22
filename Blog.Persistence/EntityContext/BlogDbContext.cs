using Microsoft.EntityFrameworkCore;
using Blog.Domain.Models;
using Blog.Application.Interfaces;
using Blog.Persistence.ModelsConfigurations;

namespace Blog.Persistence.EntityContext
{
    public class BlogDbContext : DbContext , IBlogDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }    

        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ArticleConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
