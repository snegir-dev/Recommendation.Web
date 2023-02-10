using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendation.Domain;

namespace Recommendation.Persistence.EntityTypeConfigurations;

public class ImageInfoConfiguration : IEntityTypeConfiguration<ImageInfo>
{
    public void Configure(EntityTypeBuilder<ImageInfo> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasIndex(i => i.Id);
    }
}