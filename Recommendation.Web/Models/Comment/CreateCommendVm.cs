using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Comment.Commands.Create;

namespace Recommendation.Web.Models.Comment;

public class CreateCommendVm : IMapWith<CreateCommentCommand>
{
    public Guid ReviewId { get; set; }
    public string Comment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCommendVm, CreateCommentCommand>()
            .ForMember(c => c.ReviewId,
                exp => exp.MapFrom(c => c.ReviewId))
            .ForMember(c => c.Comment,
                exp => exp.MapFrom(c => c.Comment));
    }
}