namespace PCBack.Models;

/// <summary>
/// Maps persisted <see cref="PatentAnalysis"/> rows to API <see cref="CommercialReport"/> (same shape as POST /analyze).
/// </summary>
public static class PatentAnalysisMapper
{
    public static CommercialReport ToCommercialReport(PatentAnalysis row)
    {
        return new CommercialReport
        {
            Title = row.Title ?? string.Empty,
            PatentOwner = row.PatentOwner ?? string.Empty,
            PatentStatus = row.PatentStatus ?? string.Empty,
            TechnologyTags = SplitCsv(row.TechnologyTags),
            PotentialMarkets = SplitCsv(row.PotentialMarkets),
            CommercialOpportunities = SplitCsv(row.CommercialOpportunities)
        };
    }

    private static List<string> SplitCsv(string? stored)
    {
        if (string.IsNullOrWhiteSpace(stored))
            return new List<string>();

        return stored.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
