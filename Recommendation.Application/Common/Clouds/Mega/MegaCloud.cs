using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Http;

namespace Recommendation.Application.Common.Clouds.Mega;

public class MegaCloud : IMegaCloud
{
    private readonly MegaApiClient _megaApiClient;

    private readonly Guid _newFolderName = Guid.NewGuid();
    private const string BaseCloudPath = "RecommendationWeb";

    public MegaCloud(string email, string password)
    {
        _megaApiClient = new MegaApiClient();
        _megaApiClient.Login(email, password);
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        var nodes = await _megaApiClient.GetNodesAsync();
        var root = nodes.Single(x => x.Name == BaseCloudPath);
        var folder = await CreateFolder(_newFolderName.ToString(), root);
        var stream = await GetStreamFile(file);

        var fileNode = await _megaApiClient.UploadAsync(stream, file.FileName, folder);
        var uri = await _megaApiClient.GetDownloadLinkAsync(fileNode);

        return uri.AbsoluteUri;
    }

    private static async Task<Stream> GetStreamFile(IFormFile file)
    {
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        return stream;
    }

    private async Task<INode> CreateFolder(string name, INode parentNode)
    {
        var node = await _megaApiClient.CreateFolderAsync(name, parentNode)
                   ?? throw new NullReferenceException("Failed to create folder");
        return node;
    }
}