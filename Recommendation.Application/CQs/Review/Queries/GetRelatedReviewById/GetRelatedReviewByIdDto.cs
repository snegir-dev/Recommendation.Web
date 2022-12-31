using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Review.Queries.GetRelatedReviewById;

public class GetRelatedReviewByIdDto : IMapWith<Domain.Review>
{
    public Guid Id { get; set; }
    public string NameReview { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Review, GetRelatedReviewByIdDto>();
    }
}