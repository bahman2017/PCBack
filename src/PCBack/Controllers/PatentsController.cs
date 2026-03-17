using Microsoft.AspNetCore.Mvc;
using PCBack.Models;
using PCBack.Services;

namespace PCBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatentsController : ControllerBase
{
    private readonly IPatentService _patentService;
    private readonly IAiAnalysisService _aiAnalysisService;

    public PatentsController(IPatentService patentService, IAiAnalysisService aiAnalysisService)
    {
        _patentService = patentService;
        _aiAnalysisService = aiAnalysisService;
    }

    /// <summary>
    /// Analyzes a patent by patent number and/or abstract and returns a commercial report.
    /// </summary>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(CommercialReport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommercialReport>> Analyze([FromBody] PatentAnalysisRequest request)
    {
        if (request == null)
            return BadRequest("Request body is required.");

        string abstractToUse;
        PatentMetadata? metadata = null;

        if (!string.IsNullOrWhiteSpace(request.PatentNumber))
        {
            metadata = await _patentService.GetPatentMetadataAsync(request.PatentNumber.Trim());
            abstractToUse = metadata?.Abstract ?? request.Abstract ?? string.Empty;
        }
        else if (!string.IsNullOrWhiteSpace(request.Abstract))
        {
            abstractToUse = request.Abstract.Trim();
        }
        else
        {
            return BadRequest("Either PatentNumber or Abstract must be provided.");
        }

        var report = await _aiAnalysisService.GenerateCommercialReportAsync(abstractToUse);

        if (metadata != null)
        {
            if (!string.IsNullOrWhiteSpace(metadata.Title))
                report.Title = metadata.Title;
            if (!string.IsNullOrWhiteSpace(metadata.PatentOwner))
                report.PatentOwner = metadata.PatentOwner;
            if (!string.IsNullOrWhiteSpace(metadata.PatentStatus))
                report.PatentStatus = metadata.PatentStatus;
        }

        return Ok(report);
    }
}
