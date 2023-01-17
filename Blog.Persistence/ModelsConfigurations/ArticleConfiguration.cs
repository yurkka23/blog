//using Blog.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Blog.Persistence.ModelsConfigurations;

//public class ArticleConfiguration : IEntityTypeConfiguration<Article>
//{
//    public void Configure(EntityTypeBuilder<Article> builder)
//    {
//        builder.HasKey(article => article.Id);

//        builder.HasIndex(article => article.Id)
//            .IsUnique();

//        builder.Property(article => article.Title)
//            .IsRequired()
//            .HasMaxLength(50);

//        builder.Property(article => article.Content)
//            .IsRequired()
//            .HasMaxLength(1000);

//        builder.Property(article => article.Genre)
//            .IsRequired()
//            .HasMaxLength(15);

//        builder.HasMany<Comment>(article => article.Comments)
//            .WithOne(comment => comment.Article)
//            .HasForeignKey(comment => comment.ArticleId)
//            .OnDelete(DeleteBehavior.Cascade);

//        builder.HasMany<Rating>(article => article.Ratings)
//            .WithOne(rating => rating.Article)
//            .HasForeignKey(rating => rating.ArticleId)
//            .OnDelete(DeleteBehavior.Cascade);
//    }
//}
