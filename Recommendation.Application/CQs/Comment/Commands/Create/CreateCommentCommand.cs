using AutoMapper;
using MediatR;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Comment.Commands.Create;

public class CreateCommentCommand : IRequest<Guid>, IMapWith<Domain.Comment>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public string Comment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCommentCommand, Domain.Comment>()
            .ForMember(c => c.Description,
                exp => exp.MapFrom(c => c.Comment))
            .ForMember(c => c.DateCreation,
                exp => exp.MapFrom(_ => DateTime.UtcNow));
    }
}