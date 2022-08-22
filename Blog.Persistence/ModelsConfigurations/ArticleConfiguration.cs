using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistence.ModelsConfigurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(article => article.Id);

            builder.HasIndex(article => article.Id)
                .IsUnique();

            builder.Property(article => article.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(article => article.Content)
                .IsRequired()
                .HasMaxLength(100_000);

           
        }
    }
}
