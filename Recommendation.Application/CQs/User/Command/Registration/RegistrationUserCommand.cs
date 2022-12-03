using AutoMapper;
using MediatR;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.User.Command.Registration;

public class RegistrationUserCommand : IRequest, IMapWith<Domain.User>
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
    public bool IsRemember { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegistrationUserCommand, Domain.User>()
            .ForMember(u => u.Id,
                c => c.MapFrom(_ => Guid.NewGuid()))
            .ForMember(u => u.UserName,
                c => c.MapFrom(u => u.Login))
            .ForMember(u => u.Email,
                c => c.MapFrom(u => u.Email));
    }
}