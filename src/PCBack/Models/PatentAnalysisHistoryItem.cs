namespace PCBack.Models;

/// <summary>
/// API shape for persisted analysis history (lists restored from comma-separated storage).
/// </summary>
public class PatentAnalysisHistoryItem
{
    public Guid Id { get; set; }
    public string? PatentNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PatentOwner { get; set; } = string.Empty;
    public string PatentStatus { get; set; } = string.Empty;
    public List<string> TechnologyTags { get; set; } = new();
    public List<string> PotentialMarkets { get; set; } = new();
    public List<string> CommercialOpportunities { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
