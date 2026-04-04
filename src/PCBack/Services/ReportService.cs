using Microsoft.EntityFrameworkCore;
using PCBack.Data;
using PCBack.Models;

namespace PCBack.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _dbContext;

    public ReportService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommercialReport?> GetByIdAsync(Guid id)
    {
        var row = await _dbContext.PatentAnalyses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return row == null ? null : PatentAnalysisMapper.ToCommercialReport(row);
    }
}
