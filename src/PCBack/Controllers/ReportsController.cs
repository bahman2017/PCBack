using Microsoft.AspNetCore.Mvc;
using PCBack.Services;

namespace PCBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IPdfService _pdfService;

    public ReportsController(IReportService reportService, IPdfService pdfService)
    {
        _reportService = reportService;
        _pdfService = pdfService;
    }

    /// <summary>
    /// Downloads a PDF of the persisted commercial report with the given id.
    /// </summary>
    [HttpGet("{id:guid}/pdf")]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPdf(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null)
            return NotFound();

        var pdfBytes = _pdfService.GenerateReportPdf(report);
        return File(pdfBytes, "application/pdf", $"report-{id}.pdf");
    }
}
