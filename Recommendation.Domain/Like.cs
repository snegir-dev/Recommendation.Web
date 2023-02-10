﻿namespace Recommendation.Domain;

public class Like : IBaseEntity
{
    public Guid Id { get; set; }
    public bool IsLike { get; set; }
    
    public UserApp User { get; set; }
    public Review Review { get; set; }
}