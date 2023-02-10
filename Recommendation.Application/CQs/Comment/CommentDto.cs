using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Comment;

public class CommentDto : IMapWith<Domain.Comment>
{
    public string AuthorName { get; set; }
    public int CountUserLike { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Comment, CommentDto>()
            .ForMember(c => c.AuthorName,
                exp => exp.MapFrom(c => c.User.UserName))
            .ForMember(c => c.CountUserLike,
                exp => exp.MapFrom(c => c.User.CountLike));
    }
}