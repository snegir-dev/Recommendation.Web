using System.Dynamic;
using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Domain;

namespace Recommendation.Application.Common.AlgoliaSearch.Entities;

public class AlgoliaBaseEntity : IMapWith<Review>
{
    public string ObjectID { get; set; }
    public string ReviewName { get; set; }
    public string ReviewDescription { get; set; }
    public int ReviewAuthorGrade { get; set; }
    public string UserName { get; set; }
    public string CompositionName { get; set; }
    public double CompositionAverageRating { get; set; }
    public List<string> CommentDescriptions { get; set; }
    public List<string> TagNames { get; set; }
    public string CategoryName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, AlgoliaBaseEntity>()
            .ForMember(a => a.ObjectID,
                c => c.MapFrom(o => ((IBaseEntity)o).Id))
            .ForMember(a => a.ReviewName,
                c => c.MapFrom(r => r.NameReview))
            .ForMember(a => a.ReviewDescription,
                c => c.MapFrom(r => r.Description))
            .ForMember(a => a.ReviewAuthorGrade,
                c => c.MapFrom(r => r.AuthorGrade))
            .ForMember(a => a.UserName,
                c => c.MapFrom(r => r.User.UserName))
            .ForMember(a => a.CompositionName,
                c => c.MapFrom(r => r.Composition.Name))
            .ForMember(a => a.CompositionAverageRating,
                c => c.MapFrom(r => r.Composition.AverageRating))
            .ForMember(a => a.CommentDescriptions,
                c => c.MapFrom(r => r.Comments.Select(rc => rc.Description)))
            .ForMember(a => a.TagNames,
                c => c.MapFrom(r => r.Tags.Select(t => t.Name)))
            .ForMember(a => a.CategoryName,
                c => c.MapFrom(r => r.Category.Name));
    }
}