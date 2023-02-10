using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendation.Domain;

namespace Recommendation.Persistence.EntityTypeConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.Id);
        builder.Property(r => r.Description).HasMaxLength(10000);
        builder.Property(r => r.NameReview).HasMaxLength(100);
    }
}