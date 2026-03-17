namespace PCBack.Models;

public class CommercialReport
{
    public string Title { get; set; } = string.Empty;
    public string PatentOwner { get; set; } = string.Empty;
    public string PatentStatus { get; set; } = string.Empty;
    public List<string> TechnologyTags { get; set; } = new();
    public List<string> PotentialMarkets { get; set; } = new();
    public List<string> CommercialOpportunities { get; set; } = new();
}
