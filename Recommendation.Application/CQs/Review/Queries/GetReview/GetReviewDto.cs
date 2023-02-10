using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Queries.GetReview;

public class GetReviewDto : IMapWith<Domain.Review>
{
    public Guid ReviewId { get; set; }
    public string Author { get; set; }
    public string[]? UrlImages { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public double AverageCompositionRate { get; set; }
    public int OwnSetRating { get; set; }
    public bool IsLike { get; set; }
    public int CountLike { get; set; }
    public int CountLikeAuthor { get; set; }
    public DateTime DateCreation { get; set; }
    public List<string> Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Review, GetReviewDto>()
            .ForMember(r => r.ReviewId,
                c => c.MapFrom(r => r.Id))
            .ForMember(r => r.UrlImages,
                c => c.MapFrom(r => r.ImageInfos != null && r.ImageInfos.Count > 0
                    ? r.ImageInfos.Select(i => i.Url) : null))
            .ForMember(r => r.Author,
                c => c.MapFrom(r => r.User.UserName))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.Composition.Name))
            .ForMember(r => r.AverageCompositionRate,
                c => c.MapFrom(r => r.Composition.AverageRating))
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category.Name))
            .ForMember(r => r.CountLikeAuthor,
                c => c.MapFrom(r => r.User.CountLike))
            .ForMember(r => r.Tags,
                c => c.MapFrom(r => r.Tags.Select(t => t.Name)));
    }
}