using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBack.Data;
using PCBack.Models;
using PCBack.Services;

namespace PCBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatentsController : ControllerBase
{
    private readonly IPatentService _patentService;
    private readonly IAiAnalysisService _aiAnalysisService;
    private readonly IReportService _reportService;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PatentsController> _logger;

    public PatentsController(
        IPatentService patentService,
        IAiAnalysisService aiAnalysisService,
        IReportService reportService,
        ApplicationDbContext dbContext,
        ILogger<PatentsController> logger)
    {
        _patentService = patentService;
        _aiAnalysisService = aiAnalysisService;
        _reportService = reportService;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Returns a persisted commercial report by id (same JSON shape as POST /analyze).
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommercialReport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommercialReport>> GetById(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null)
            return NotFound();

        return Ok(report);
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
        string? patentNumberInput = null;

        if (!string.IsNullOrWhiteSpace(request.PatentNumber))
        {
            patentNumberInput = request.PatentNumber.Trim();
            metadata = await _patentService.GetPatentMetadataAsync(patentNumberInput);
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

        try
        {
            await PersistReportAsync(report, patentNumberInput);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to persist patent analysis report; returning response without saving.");
        }

        return Ok(report);
    }

    /// <summary>
    /// Returns the 20 most recent persisted patent analyses.
    /// </summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IReadOnlyList<PatentAnalysisHistoryItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PatentAnalysisHistoryItem>>> History()
    {
        try
        {
            var rows = await _dbContext.PatentAnalyses
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .ToListAsync();

            var items = rows.Select(MapToHistoryItem).ToList();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load patent analysis history.");
            return Ok(Array.Empty<PatentAnalysisHistoryItem>());
        }
    }

    private async Task PersistReportAsync(CommercialReport report, string? patentNumber)
    {
        var entity = new PatentAnalysis
        {
            Id = Guid.NewGuid(),
            PatentNumber = patentNumber,
            Title = report.Title ?? string.Empty,
            PatentOwner = report.PatentOwner ?? string.Empty,
            PatentStatus = report.PatentStatus ?? string.Empty,
            TechnologyTags = JoinList(report.TechnologyTags),
            PotentialMarkets = JoinList(report.PotentialMarkets),
            CommercialOpportunities = JoinList(report.CommercialOpportunities),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.PatentAnalyses.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    private static string JoinList(IReadOnlyList<string>? list)
    {
        if (list == null || list.Count == 0)
            return string.Empty;

        return string.Join(",", list.Where(s => !string.IsNullOrWhiteSpace(s)));
    }

    private static PatentAnalysisHistoryItem MapToHistoryItem(PatentAnalysis row)
    {
        return new PatentAnalysisHistoryItem
        {
            Id = row.Id,
            PatentNumber = row.PatentNumber,
            Title = row.Title,
            PatentOwner = row.PatentOwner,
            PatentStatus = row.PatentStatus,
            TechnologyTags = SplitList(row.TechnologyTags),
            PotentialMarkets = SplitList(row.PotentialMarkets),
            CommercialOpportunities = SplitList(row.CommercialOpportunities),
            CreatedAt = row.CreatedAt
        };
    }

    private static List<string> SplitList(string? stored)
    {
        if (string.IsNullOrWhiteSpace(stored))
            return new List<string>();

        return stored.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
