using PCBack.Models;
using PCBack.Services;

namespace PCBack.Tests.Fakes;

/// <summary>
/// Test double: deterministic report without OpenAI or prompt pipeline.
/// </summary>
public class FakeAiAnalysisService : IAiAnalysisService
{
    public Task<CommercialReport> GenerateCommercialReportAsync(string abstractText)
    {
        var report = new CommercialReport
        {
            Title = string.Empty,
            PatentOwner = string.Empty,
            PatentStatus = string.Empty,
            TechnologyTags = new List<string> { "AI" },
            PotentialMarkets = new List<string> { "Healthcare" },
            CommercialOpportunities = new List<string> { "Licensing" }
        };

        return Task.FromResult(report);
    }
}
