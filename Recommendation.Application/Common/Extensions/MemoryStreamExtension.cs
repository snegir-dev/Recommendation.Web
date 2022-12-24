using Microsoft.AspNetCore.Http;

namespace Recommendation.Application.Common.Extensions;

public static class MemoryStreamExtension
{
    public static IFormFile ToFormFile(this MemoryStream stream, string name, string contentType)
    {
        var formFile = new FormFile(stream, 0, stream.Length, name, name)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        return formFile;
    }
}