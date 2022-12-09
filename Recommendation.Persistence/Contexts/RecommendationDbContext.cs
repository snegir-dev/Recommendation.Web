using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Persistence.Contexts;

public sealed class RecommendationDbContext
    : IdentityDbContext<UserApp, IdentityRole<Guid>, Guid>, IRecommendationDbContext
{
    public DbSet<UserApp> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }
}