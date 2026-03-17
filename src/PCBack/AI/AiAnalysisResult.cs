using System.Text.Json.Serialization;

namespace PCBack.AI;

/// <summary>
/// Structured result from the AI analysis pipeline (JSON response).
/// </summary>
public class AiAnalysisResult
{
    [JsonPropertyName("technologyTags")]
    public List<string> TechnologyTags { get; set; } = new();

    [JsonPropertyName("potentialMarkets")]
    public List<string> PotentialMarkets { get; set; } = new();

    [JsonPropertyName("commercialOpportunities")]
    public List<string> CommercialOpportunities { get; set; } = new();
}
