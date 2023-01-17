//using Microsoft.EntityFrameworkCore;
//using Blog.Domain.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Blog.Domain;
//using System.Reflection;

//namespace Blog.Persistence.EntityContext;

//public class BlogDbContext :  IdentityDbContext<User, ApplicationRole, Guid> //,IBlogDbContext
//{
//    //public DbSet<User> Users { get; set; }//because it's already been in IdentityDb
//    public DbSet<Article> Articles { get; set; }
//    public DbSet<Rating> Ratings { get; set; }
//    public DbSet<Comment> Comments { get; set; }
//    public DbSet<UserSubscription> UserSubscriptions { get; set; }
//    public DbSet<Message> Messages { get; set; }
//  //  public DbSet<Group> Groups { get; set; }
//    public DbSet<Connection> Connections { get; set; }

//    public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options) 
//    {
//        //Database.EnsureCreated();
//     //Database.Migrate();//to update-migration 
//    }

//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        builder.ApplyConfigurationsFromAssembly( Assembly.GetExecutingAssembly() );
//        base.OnModelCreating(builder);
//    }
    
//}
