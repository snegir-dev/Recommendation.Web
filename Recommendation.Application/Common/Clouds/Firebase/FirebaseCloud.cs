using System.Collections;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Clouds.Firebase.Entities;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Recommendation.Application.Common.Clouds.Firebase;

public class FirebaseCloud
{
    private readonly StorageClient _storageClient;
    private readonly string _bucket;

    public FirebaseCloud(JsonCredentialParameters credentialParameters, string bucket)
    {
        _bucket = bucket;
        _storageClient = StorageClient
            .Create(GoogleCredential.FromJsonParameters(credentialParameters));
    }

    public async Task<IEnumerable<ImageMetadata>> UploadFiles(IEnumerable<IFormFile> files,
        string folderName)
    {
        var imageMetadatas = new List<ImageMetadata>();
        foreach (var file in files)
        {
            var imageMetadata = await UploadFile(file, folderName);
            imageMetadatas.Add(imageMetadata);
        }

        return imageMetadatas;
    }

    public async Task<ImageMetadata> UploadFile(IFormFile file, string folderName)
    {
        await CreateFolder(folderName);
        var path = $"{folderName}/{file.FileName}";

        var stream = await GetStreamFile(file);
        var response = await _storageClient
            .UploadObjectAsync(_bucket, path, file.ContentType, stream);

        return new ImageMetadata(file.FileName, folderName, response.MediaLink);
    }

    public async Task<IEnumerable<ImageMetadata>> UpdateFiles(IEnumerable<IFormFile> files,
        string folderName)
    {
        var imageMetadatas = new List<ImageMetadata>();
        await DeleteFolder(folderName);
        foreach (var file in files)
        {
            var imageMetadata = await UploadFile(file, folderName);
            imageMetadatas.Add(imageMetadata);
        }

        return imageMetadatas;
    }

    public async Task DeleteFolder(string? folderName)
    {
        if (folderName == null) return;
        var folders = _storageClient
            .ListObjectsAsync(_bucket, folderName)
            .DefaultIfEmpty()
            .ToEnumerable();

        foreach (var folder in folders)
        {
            if (folder != null)
                await _storageClient.DeleteObjectAsync(folder);
        }
    }

    private async Task CreateFolder(string name)
    {
        const string folderCreationContentType = "application/x-directory";
        await _storageClient.UploadObjectAsync(_bucket, $"{name}/",
            folderCreationContentType, new MemoryStream());
    }

    private static async Task<Stream> GetStreamFile(IFormFile file)
    {
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        return stream;
    }
}