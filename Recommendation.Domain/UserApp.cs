﻿using Microsoft.AspNetCore.Identity;

namespace Recommendation.Domain;

public class UserApp : IdentityUser<Guid>, IBaseEntity
{
    public override Guid Id { get; set; }
    public int CountLike { get; set; }
    public string AccessStatus { get; set; }

    public List<Like> Likes { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}