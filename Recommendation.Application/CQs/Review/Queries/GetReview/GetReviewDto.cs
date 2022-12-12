using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Queries.GetReview;

public class GetReviewDto : IMapWith<Domain.Review>
{
    public Guid ReviewId { get; set; }
    public string Author { get; set; }
    public string UrlImage { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public double AverageCompositionRate { get; set; }
    public int OwnSetRating { get; set; }
    public bool IsLike { get; set; }
    public List<string> Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Review, GetReviewDto>()
            .ForMember(r => r.ReviewId,
                c => c.MapFrom(r => r.Id))
            .ForMember(r => r.Author,
                c => c.MapFrom(r => r.User.UserName))
            .ForMember(r => r.UrlImage,
                c => c.MapFrom(r => r.UrlImage))
            .ForMember(r => r.NameReview,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.Composition.Name))
            .ForMember(r => r.Description,
                c => c.MapFrom(r => r.Description))
            .ForMember(r => r.AuthorGrade,
                c => c.MapFrom(r => r.AuthorGrade))
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category.Name))
            .ForMember(r => r.Tags,
                c => c.MapFrom(r => r.Tags.Select(t => t.Name)));
    }
}