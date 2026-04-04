using PCBack.Models;

namespace PCBack.Services;

public interface IReportService
{
    Task<CommercialReport?> GetByIdAsync(Guid id);
}
