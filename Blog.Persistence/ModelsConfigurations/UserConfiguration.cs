using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistence.ModelsConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.HasIndex(user => user.Id)
                .IsUnique();

            builder.Property(user => user.AboutMe)
                .HasMaxLength(600);

            builder.Property(user => user.UserName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(user => user.FirstName)
                .HasMaxLength(20);

            builder.Property(user => user.LastName)
                .HasMaxLength(20);

            builder.HasMany<Article>(user => user.Articles)
               .WithOne(article => article.User)
               .HasForeignKey(article => article.UserId);
        }
    }
}
