using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.User.Queries.Login;

namespace Recommendation.Web.Models.User;

public class LoginUserDto : IMapWith<LoginUserQuery>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsRemember { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<LoginUserDto, LoginUserQuery>()
            .ForMember(u => u.Email,
                c => c.MapFrom(u => u.Email))
            .ForMember(u => u.Password,
                c => c.MapFrom(u => u.Password))
            .ForMember(u => u.IsRemember,
                c => c.MapFrom(u => u.IsRemember));
    }
}