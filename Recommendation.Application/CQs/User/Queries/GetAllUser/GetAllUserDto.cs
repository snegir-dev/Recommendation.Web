using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetAllUser;

public class GetAllUserDto : IMapWith<UserApp>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int UserLikeCount { get; set; }
    public string Role { get; set; }
    public string AccessStatus { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserApp, GetAllUserDto>()
            .ForMember(u => u.Id,
                c => c.MapFrom(u => u.Id))
            .ForMember(u => u.Name,
                c => c.MapFrom(u => u.UserName))
            .ForMember(u => u.Email,
                c => c.MapFrom(u => u.Email))
            .ForMember(u => u.UserLikeCount,
                c => c.MapFrom(u => u.CountLike));
    }
}