using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Review.Commands.Create;

namespace Recommendation.Web.Models.Review;

public class CreateReviewDto : IMapWith<CreateReviewCommand>
{
    public IFormFile[]? Images { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public string[] Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateReviewDto, CreateReviewCommand>()
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category))
            .ForMember(r => r.NameReview,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(r => r.AuthorGrade,
                c => c.MapFrom(r => r.AuthorGrade))
            .ForMember(r => r.Description,
                c => c.MapFrom(r => r.Description))
            .ForMember(r => r.Images,
                c => c.MapFrom(r => r.Images))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.NameDescription));
    }
}