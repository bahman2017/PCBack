using PCBack.Models;

namespace PCBack.Services;

public interface IPdfService
{
    byte[] GenerateReportPdf(CommercialReport report);
}
