using PCBack.Models;

namespace PCBack.Services;

public interface IAiAnalysisService
{
    Task<CommercialReport> GenerateCommercialReportAsync(string abstractText);
}
