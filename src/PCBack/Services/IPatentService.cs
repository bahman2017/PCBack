using PCBack.Models;

namespace PCBack.Services;

public interface IPatentService
{
    Task<PatentMetadata?> GetPatentMetadataAsync(string patentNumber);
}
