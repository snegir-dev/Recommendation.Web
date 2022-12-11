using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Comment.Queries.GetAllComment;

public class GetAllCommentDto : IMapWith<Domain.Comment>
{
    public string AuthorName { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Comment, GetAllCommentDto>()
            .ForMember(c => c.AuthorName,
                exp => exp.MapFrom(c => c.User.UserName))
            .ForMember(c => c.Description,
                exp => exp.MapFrom(c => c.Description))
            .ForMember(c => c.DateCreation,
                exp => exp.MapFrom(c => c.DateCreation));
    }
}