using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Review.Queries.GetReview;

namespace Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

public class GetUpdatedReviewDto : IMapWith<Domain.Review>
{
    public Guid ReviewId { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public List<string> Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Review, GetUpdatedReviewDto>()
            .ForMember(r => r.ReviewId,
                c => c.MapFrom(r => r.Id))
            .ForMember(r => r.AuthorGrade,
                c => c.MapFrom(r => r.AuthorGrade))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.Composition.Name))
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category.Name))
            .ForMember(r => r.Tags,
                c => c.MapFrom(r => r.Tags.Select(t => t.Name)));
    }
}