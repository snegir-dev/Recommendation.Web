﻿using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;

public class GetAllReviewByUserIdDto : IMapWith<Domain.Review>
{
    public Guid ReviewId { get; set; }
    public string UrlImage { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public double AverageCompositionRate { get; set; }
    public string Category { get; set; }
    public int CountLike { get; set; }
    public DateTime DateCreation { get; set; }
    public string[] Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Review, GetAllReviewByUserIdDto>()
            .ForMember(r => r.ReviewId,
                c => c.MapFrom(r => r.Id))
            .ForMember(r => r.UrlImage,
                c => c.MapFrom(r => r.UrlImage))
            .ForMember(r => r.NameReview,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.Composition.Name))
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category.Name))
            .ForMember(r => r.Tags,
                c => c.MapFrom(r => r.Tags.Select(t => t.Name)))
            .ForMember(r => r.AverageCompositionRate,
                c => c.MapFrom(r => r.Composition.Ratings
                    .Select(cr => cr.RatingValue).DefaultIfEmpty().Average()))
            .ForMember(r => r.CountLike,
                c => c.MapFrom(cr => cr.Likes.Count));;
    }
}