using PCBack.Models;

namespace PCBack.Services;

public class PatentService : IPatentService
{
    public Task<PatentMetadata?> GetPatentMetadataAsync(string patentNumber)
    {
        // Placeholder: no external API integration yet.
        return Task.FromResult<PatentMetadata?>(new PatentMetadata
        {
            Title = $"Patent {patentNumber}",
            PatentOwner = "Placeholder Owner",
            PatentStatus = "Active",
            Abstract = $"Placeholder abstract for patent {patentNumber}."
        });
    }
}
