using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Rating.Commands.SetRating;

namespace Recommendation.Web.Models.Grade;

public class SetRatingVm : IMapWith<SetRatingCommand>
{
    public Guid ReviewId { get; set; }
    public int GradeValue { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetRatingVm, SetRatingCommand>()
            .ForMember(g => g.ReviewId,
                c => c.MapFrom(g => g.ReviewId))
            .ForMember(g => g.RatingValue,
                c => c.MapFrom(g => g.GradeValue));
    }
}