using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.User.Command.Registration;

namespace Recommendation.Web.Models.User;

public class RegistrationUserDto : IMapWith<RegistrationUserCommand>
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    public bool IsRemember { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegistrationUserDto, RegistrationUserCommand>()
            .ForMember(u => u.Login,
                c => c.MapFrom(u => u.Login))
            .ForMember(u => u.Email,
                c => c.MapFrom(u => u.Email))
            .ForMember(u => u.Password,
                c => c.MapFrom(u => u.Password))
            .ForMember(u => u.PasswordConfirmation,
                c => c.MapFrom(u => u.PasswordConfirmation))
            .ForMember(u => u.IsRemember,
                c => c.MapFrom(u => u.IsRemember));
    }
}