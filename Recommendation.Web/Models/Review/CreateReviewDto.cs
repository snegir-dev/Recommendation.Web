using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Review.Create;

namespace Recommendation.Web.Models.Review;

public class CreateReviewDto : IMapWith<CreateReviewCommand>
{
    public IFormFile Image { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int Grade { get; set; }

    public string Category { get; set; }
    public string Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateReviewDto, CreateReviewCommand>()
            .ForMember(r => r.Category,
                c => c.MapFrom(r => r.Category))
            .ForMember(r => r.NameReview,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(r => r.Tags,
                c => c.MapFrom(r => r.Tags.Split(new[] { ',' })))
            .ForMember(r => r.Grade,
                c => c.MapFrom(r => r.Grade))
            .ForMember(r => r.Description,
                c => c.MapFrom(r => r.Description))
            .ForMember(r => r.Image,
                c => c.MapFrom(r => r.Image))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.NameDescription));
    }
}