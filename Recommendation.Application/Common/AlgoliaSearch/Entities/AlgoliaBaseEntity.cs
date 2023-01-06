using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Domain;

namespace Recommendation.Application.Common.AlgoliaSearch.Entities;

public class AlgoliaBaseEntity : IMapWith<IBaseEntity>
{
    public string ObjectID { get; set; }
    public object Entity { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<object, AlgoliaBaseEntity>()
            .ForMember(a => a.ObjectID,
                c => c.MapFrom(o => ((IBaseEntity)o).Id))
            .ForMember(a => a.Entity,
                c => c.MapFrom(o => o));
    }
}