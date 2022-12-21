using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;

namespace Recommendation.Application.Common.AlgoliaSearch;

public class AlgoliaSearchClient
{
    public SearchIndex SearchIndex { get; set; }

    private readonly ISearchClient _searchClient;

    public AlgoliaSearchClient(ISearchClient searchClient)
    {
        _searchClient = searchClient;
        SetIndex();
    }

    public void SetIndex(string index = "reviews")
    {
        SearchIndex = _searchClient.InitIndex(index);
    }

    public async Task<List<T>> Search<T>(string? searchValue)
        where T : class
    {
        if (searchValue == null)
            return new List<T>();

        var response = await SearchIndex
            .SearchAsync<T>(new Query()
            {
                SearchQuery = searchValue,
                TypoTolerance = false
            });

        return response.Hits;
    }

    public async Task AddOrUpdateEntity<T>(T entity, string objectId)
    {
        if (entity != null)
        {
            ((dynamic)entity).ObjectID = objectId;
            await SearchIndex.SaveObjectAsync((dynamic)entity);
        }
    }

    public async Task DeleteEntity<T>(T entity)
    {
        if (entity != null)
            await SearchIndex.DeleteObjectAsync(((dynamic)entity).Id.ToString());
    }
}