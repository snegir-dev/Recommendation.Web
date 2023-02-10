using System.Collections;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Clouds.Firebase.Entities;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Recommendation.Application.Common.Clouds.Firebase;

public class FirebaseCloud : IFirebaseCloud
{
    private readonly StorageClient _storageClient;
    private readonly string _bucket;

    public FirebaseCloud(JsonCredentialParameters credentialParameters, string bucket)
    {
        _bucket = bucket;
        _storageClient = StorageClient
            .Create(GoogleCredential.FromJsonParameters(credentialParameters));
    }

    public async Task<IEnumerable<ImageMetadata>> UploadFilesAsync(IEnumerable<IFormFile> files,
        string folderName)
    {
        var imageMetadatas = new List<ImageMetadata>();
        foreach (var file in files)
        {
            var imageMetadata = await UploadFileAsync(file, folderName);
            imageMetadatas.Add(imageMetadata);
        }

        return imageMetadatas;
    }

    public async Task<ImageMetadata> UploadFileAsync(IFormFile file, string folderName)
    {
        await CreateFolderAsync(folderName);
        var path = $"{folderName}/{file.FileName}";

        var stream = await GetStreamFileAsync(file);
        var response = await _storageClient
            .UploadObjectAsync(_bucket, path, file.ContentType, stream);

        return new ImageMetadata(file.FileName, folderName, response.MediaLink);
    }

    public async Task<IEnumerable<ImageMetadata>> UpdateFilesAsync(IEnumerable<IFormFile> files,
        string folderName)
    {
        var imageMetadatas = new List<ImageMetadata>();
        await DeleteFolderAsync(folderName);
        foreach (var file in files)
        {
            var imageMetadata = await UploadFileAsync(file, folderName);
            imageMetadatas.Add(imageMetadata);
        }

        return imageMetadatas;
    }

    public async Task DeleteFolderAsync(string? folderName)
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

    private async Task CreateFolderAsync(string name)
    {
        const string folderCreationContentType = "application/x-directory";
        await _storageClient.UploadObjectAsync(_bucket, $"{name}/",
            folderCreationContentType, new MemoryStream());
    }

    private static async Task<Stream> GetStreamFileAsync(IFormFile file)
    {
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        return stream;
    }
}