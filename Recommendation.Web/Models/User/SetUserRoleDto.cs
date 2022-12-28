using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.User.Command.SetUserRole;

namespace Recommendation.Web.Models.User;

public class SetUserRoleDto : IMapWith<SetUserRoleCommand>
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetUserRoleDto, SetUserRoleCommand>();
    }
}