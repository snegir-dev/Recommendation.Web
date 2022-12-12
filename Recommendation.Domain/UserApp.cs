using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Recommendation.Domain;

public class UserApp : IdentityUser<Guid>
{
    public override Guid Id { get; set; }

    public List<Like> Likes { get; set; } = new();
    public List<Rating> Grades { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}