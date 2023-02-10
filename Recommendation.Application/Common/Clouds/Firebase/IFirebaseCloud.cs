using Microsoft.AspNetCore.Http;
using Recommendation.Application.Common.Clouds.Firebase.Entities;

namespace Recommendation.Application.Common.Clouds.Firebase;

public interface IFirebaseCloud
{
    Task<IEnumerable<ImageMetadata>> UploadFilesAsync(IEnumerable<IFormFile> files,
        string folderName);

    Task<ImageMetadata> UploadFileAsync(IFormFile file, string folderName);

    Task<IEnumerable<ImageMetadata>> UpdateFilesAsync(IEnumerable<IFormFile> files,
        string folderName);

    Task DeleteFolderAsync(string? folderName);
}