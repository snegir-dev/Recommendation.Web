using Recommendation.Domain;

namespace Recommendation.Application.Common.AlgoliaSearch;

public interface IAlgoliaSearchClient
{
    Task<IEnumerable<Guid>> Search(string? searchValue, string index = "reviews");
    Task AddOrUpdateEntity(Review entity, string index);
    Task DeleteEntity(string objectId, string index);
}