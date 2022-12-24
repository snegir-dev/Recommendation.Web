using System.ComponentModel.DataAnnotations.Schema;

namespace Recommendation.Domain;

public class ImageInfo
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }

    [NotMapped]
    public string PathFile => $"{FolderName}/{Name}";
    
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
}