using PCBack.Data;
using PCBack.Models;

namespace PCBack.Tests.Fakes;

/// <summary>
/// Test double: cache-by-patent-number using persisted rows (no OpenAI).
/// </summary>
public class AiAnalysisServiceFake
{
    private readonly ApplicationDbContext _db;

    public bool CacheHit { get; private set; }

    public AiAnalysisServiceFake(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<CommercialReport> GenerateAsync(string? patentNumber, string? abstractText)
    {
        if (!string.IsNullOrWhiteSpace(patentNumber))
        {
            var existing = _db.PatentAnalyses
                .Where(x => x.PatentNumber == patentNumber)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            if (existing != null)
            {
                CacheHit = true;

                return new CommercialReport
                {
                    Title = existing.Title,
                    PatentOwner = existing.PatentOwner,
                    PatentStatus = existing.PatentStatus,
                    TechnologyTags = SplitToList(existing.TechnologyTags),
                    PotentialMarkets = SplitToList(existing.PotentialMarkets),
                    CommercialOpportunities = SplitToList(existing.CommercialOpportunities)
                };
            }
        }

        CacheHit = false;

        var report = new CommercialReport
        {
            Title = string.Empty,
            PatentOwner = string.Empty,
            PatentStatus = string.Empty,
            TechnologyTags = new List<string> { "AI" },
            PotentialMarkets = new List<string> { "Healthcare" },
            CommercialOpportunities = new List<string> { "Licensing" }
        };

        var entity = new PatentAnalysis
        {
            Id = Guid.NewGuid(),
            PatentNumber = patentNumber,
            Title = report.Title,
            PatentOwner = report.PatentOwner,
            PatentStatus = report.PatentStatus,
            TechnologyTags = string.Join(",", report.TechnologyTags),
            PotentialMarkets = string.Join(",", report.PotentialMarkets),
            CommercialOpportunities = string.Join(",", report.CommercialOpportunities),
            CreatedAt = DateTime.UtcNow
        };

        _db.PatentAnalyses.Add(entity);
        await _db.SaveChangesAsync();

        return report;
    }

    private static List<string> SplitToList(string? stored)
    {
        if (string.IsNullOrWhiteSpace(stored))
            return new List<string>();

        return stored.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
