﻿using System.Collections;
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

    public async Task<ImageMetadata> UploadFile(IFormFile file, string folderName)
    {
        await CreateFolder(folderName);
        var path = $"{folderName}/{file.FileName}";

        var stream = await GetStreamFile(file);
        var response = await _storageClient
            .UploadObjectAsync(_bucket, path, file.ContentType, stream);

        return new ImageMetadata(file.FileName, folderName, response.MediaLink);
    }

    public async Task<ImageMetadata> UpdateFile(IFormFile file,
        string folderName, string oldPathFile)
    {
        await DeleteFile(oldPathFile);
        var imageMetadata = await UploadFile(file, folderName);

        return imageMetadata;
    }

    public async Task DeleteFile(string pathFile)
    {
        var file = await _storageClient.GetObjectAsync(_bucket, pathFile);
        await _storageClient.DeleteObjectAsync(file);
    }

    public async Task DeleteFolder(string folderName)
    {
        var folders = _storageClient
            .ListObjectsAsync(_bucket, folderName).AsRawResponses()
            .ToEnumerable()
            .SelectMany(o => o.Items);

        foreach (var folder in folders)
            await _storageClient.DeleteObjectAsync(folder);
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