using System.ComponentModel.DataAnnotations;

namespace Recommendation.Application.ConfigurationModels;

public class ConnectionStringsConfiguration
{
    public string RecommendationDbConnectionStringDevelop { get; set; }
    public string RecommendationDbConnectionStringProduction { get; set; }
}