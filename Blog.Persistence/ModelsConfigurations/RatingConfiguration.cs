using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Persistence.ModelsConfigurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(rating => rating.Id);

        builder.HasIndex(rating => rating.Id)
            .IsUnique();

        builder.Property(rating => rating.Score)
            .IsRequired();
    
    }
}
