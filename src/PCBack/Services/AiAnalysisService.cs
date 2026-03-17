using PCBack.Models;

namespace PCBack.Services;

public class AiAnalysisService : IAiAnalysisService
{
    public Task<CommercialReport> GenerateCommercialReportAsync(string abstractText)
    {
        // Placeholder: no external AI integration yet.
        var report = new CommercialReport
        {
            Title = "Placeholder Report Title",
            PatentOwner = "Placeholder Owner",
            PatentStatus = "Active",
            TechnologyTags = new List<string> { "Technology A", "Technology B" },
            PotentialMarkets = new List<string> { "Market 1", "Market 2" },
            CommercialOpportunities = new List<string> { "Opportunity 1", "Opportunity 2" }
        };
        return Task.FromResult(report);
    }
}
