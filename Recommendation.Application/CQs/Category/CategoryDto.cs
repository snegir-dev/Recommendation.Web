using AutoMapper;
using Recommendation.Application.Common.Mappings;

namespace Recommendation.Application.CQs.Category;

public class CategoryDto : IMapWith<Domain.Category>
{
    public string Category { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Category, CategoryDto>()
            .ForMember(category => category.Category,
                c => c.MapFrom(category => category.Name));
    }
}