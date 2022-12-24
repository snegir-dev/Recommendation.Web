using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Commands.Update;

public class UpdateReviewQuery : IRequest, IMapWith<Domain.Review>
{
    public IFormFile Image { get; set; }
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public string[] Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateReviewQuery, Domain.Review>()
            .ForMember(r => r.Id,
                c => c.MapFrom(r => r.ReviewId))
            .ForMember(r => r.Category,
                c => c.Ignore())
            .ForMember(r => r.Tags,
                c => c.Ignore());
    }
}