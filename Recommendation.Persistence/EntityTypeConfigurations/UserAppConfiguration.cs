using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendation.Domain;

namespace Recommendation.Persistence.EntityTypeConfigurations;

public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
{
    public void Configure(EntityTypeBuilder<UserApp> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Id);
        builder.HasIndex(u => u.UserName).IsUnique(false);
        builder.HasIndex(u => u.NormalizedUserName).IsUnique(false);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.UserName).HasMaxLength(100);
        builder.Property(u => u.Email);
    }
}