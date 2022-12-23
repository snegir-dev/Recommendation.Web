namespace Recommendation.Domain;

public class ImageInfo
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }
    
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
}