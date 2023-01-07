using System.Collections;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Recommendation.Application.Common.AlgoliaSearch.Entities;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.AlgoliaSearch;

public class AlgoliaSearchClient : IAlgoliaSearchClient
{
    private readonly ISearchClient _searchClient;
    private readonly IMapper _mapper;
    private readonly string[] _attributesToRetrieve = { "objectID" };
    private SearchIndex SearchIndex { get; set; }


    public AlgoliaSearchClient(ISearchClient searchClient, IMapper mapper)
    {
        _searchClient = searchClient;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Guid>> Search(string? searchValue, string index = "reviews")
    {
        SetIndex(index);
        if (searchValue == null)
            return new List<Guid>();
        var ids = await GetIdFoundRecords(searchValue);

        return ids;
    }

    public async Task AddOrUpdateEntity(Review entity, string index)
    {
        SetIndex(index);
        var algoliaEntity = _mapper.Map<Review, AlgoliaBaseEntity>(entity)
                            ?? throw new AutoMapperMappingException();

        await SearchIndex.SaveObjectAsync(algoliaEntity);
    }

    public async Task DeleteEntity(string objectId, string index)
    {
        SetIndex(index);
        await SearchIndex.DeleteObjectAsync(objectId);
    }

    private void SetIndex(string index)
    {
        SearchIndex = _searchClient.InitIndex(index);
    }

    private async Task<IEnumerable<Guid>> GetIdFoundRecords(string? searchValue)
    {
        var response = await SearchIndex
            .SearchAsync<JObject>(new Query()
            {
                SearchQuery = searchValue,
                AttributesToRetrieve = _attributesToRetrieve,
                TypoTolerance = false
            });

        var ids = await ConvertFoundIdsToGuids(response.Hits);

        return ids;
    }

    private Task<IEnumerable<Guid>> ConvertFoundIdsToGuids(IEnumerable<JObject> jObjects)
    {
        const string algoliaEntityIdName = "objectID";
        var ids = jObjects
            .Select(j => j.GetValue(algoliaEntityIdName)?.Value<string>())
            .Where(s => s != null)
            .Select(Guid.Parse!);

        return Task.FromResult(ids);
    }
}