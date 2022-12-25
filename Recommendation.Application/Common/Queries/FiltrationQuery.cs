using Recommendation.Application.Common.Constants;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Queries;

public static class FiltrationQuery
{
    public static readonly Dictionary<string, Func<IQueryable<Review>,
        IOrderedQueryable<Review>>> Filtration = new()
    {
        { FiltrationType.Date, f => f.OrderByDescending(r => r.DateCreation) },
        { FiltrationType.Rating, f => f.OrderByDescending(r => r.Composition.AverageRating) },
        { FiltrationType.Default, f => f.OrderByDescending(r => r.DateCreation) }
    };
}