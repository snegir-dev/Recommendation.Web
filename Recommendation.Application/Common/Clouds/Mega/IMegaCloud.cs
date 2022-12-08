using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Http;

namespace Recommendation.Application.Common.Clouds.Mega;

public interface IMegaCloud
{
    Task<string> UploadFile(IFormFile file);
}