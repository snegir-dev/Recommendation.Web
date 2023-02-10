using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Like.Commands;
using Recommendation.Application.CQs.Like.Commands.SetLike;

namespace Recommendation.Web.Models.Like;

public class SetLikeVm : IMapWith<SetLikeCommand>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public bool IsLike { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetLikeVm, SetLikeCommand>()
            .ForMember(l => l.UserId,
                c => c.MapFrom(l => l.UserId))
            .ForMember(l => l.ReviewId,
                c => c.MapFrom(l => l.ReviewId))
            .ForMember(l => l.IsLike,
                c => c.MapFrom(l => l.IsLike));
    }
}