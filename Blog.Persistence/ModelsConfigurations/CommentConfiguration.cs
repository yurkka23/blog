//using Blog.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Blog.Persistence.ModelsConfigurations;

//public class CommentConfiguration : IEntityTypeConfiguration<Comment>
//{
//    public void Configure(EntityTypeBuilder<Comment> builder)
//    {
//        builder.HasKey(comment => comment.Id);

//        builder.HasIndex(comment => comment.Id)
//            .IsUnique();

//        builder.Property(rating => rating.Message)
//            .IsRequired()
//            .HasMaxLength(200);

//    }
//}
