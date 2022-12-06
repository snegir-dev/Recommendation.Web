using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Persistence.Contexts;

public sealed class RecommendationDbContext
    : IdentityDbContext<User>, IRecommendationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Hashtag> Hashtags { get; set; }

    public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }
}