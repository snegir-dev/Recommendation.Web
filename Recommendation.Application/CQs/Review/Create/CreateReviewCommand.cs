using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Create;

public class CreateReviewCommand : IRequest<Guid>, IMapWith<Domain.Review>
{
    public Guid UserId { get; set; }
    public IFormFile Image { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int Grade { get; set; }
    public string Category { get; set; }
    public string[] Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateReviewCommand, Domain.Review>()
            .ForMember(r => r.NameReview,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(r => r.NameDescription,
                c => c.MapFrom(r => r.NameDescription))
            .ForMember(r => r.Description,
                c => c.MapFrom(r => r.Description))
            .ForMember(r => r.Grade,
                c => c.MapFrom(r => r.Grade))
            .ForMember(r => r.Category, 
                c => c.Ignore());
    }
}