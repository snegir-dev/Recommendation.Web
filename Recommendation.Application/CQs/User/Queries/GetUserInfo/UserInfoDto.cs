using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserInfo;

public class UserInfoDto : IMapWith<UserApp>
{
    public string Name { get; set; }
    public int CountLike { get; set; }
    public string AccessStatus { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserApp, UserInfoDto>()
            .ForMember(u => u.Name,
                c => c.MapFrom(u => u.UserName))
            .ForMember(u => u.CountLike,
                c => c.MapFrom(u => u.CountLike));
    }
}