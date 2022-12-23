using AutoMapper;
using Recommendation.Application.Common.Mappings;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Clouds.Firebase.Entities;

public class ImageMetadata : IMapWith<ImageInfo>
{
    public ImageMetadata(string name, string folderName, string url)
    {
        Name = name;
        FolderName = folderName;
        Url = url;
    }

    public string Url { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ImageMetadata, ImageInfo>();
    }
}