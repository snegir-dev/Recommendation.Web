using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.CQs.Review.Commands.Update;
using Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

namespace Recommendation.Web.Models.Review;

public class UpdatedReviewDto : IMapWith<UpdateReviewQuery>
{
    public Guid ReviewId { get; set; }
    public string NameReview { get; set; }
    public string NameDescription { get; set; }
    public string Description { get; set; }
    public int AuthorGrade { get; set; }
    public string Category { get; set; }
    public string[] Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatedReviewDto, UpdateReviewQuery>();
    }
}