using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Recommendation.Domain;

public class UserApp : IdentityUser<Guid>
{
    public override Guid Id { get; set; }

    public List<Review> Reviews { get; set; } = new();
}