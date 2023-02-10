using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Tag.Queries.GetAllTags;

public class GetAllTagsDto : IMapWith<Domain.Tag>
{
    public string Tag { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Tag, GetAllTagsDto>()
            .ForMember(t => t.Tag,
                c => c.MapFrom(t => t.Name));
    }
}