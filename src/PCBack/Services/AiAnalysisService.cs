using System.Text.Json;
using System.Text.RegularExpressions;
using PCBack.AI;
using PCBack.Models;

namespace PCBack.Services;

public class AiAnalysisService : IAiAnalysisService
{
    private readonly PromptBuilder _promptBuilder;
    private readonly IAiClient _aiClient;

    public AiAnalysisService(PromptBuilder promptBuilder, IAiClient aiClient)
    {
        _promptBuilder = promptBuilder;
        _aiClient = aiClient;
    }

    public async Task<CommercialReport> GenerateCommercialReportAsync(string abstractText)
    {
        var metadata = new PatentMetadata
        {
            Abstract = abstractText,
            Title = null,
            PatentOwner = null,
            PatentStatus = null
        };

        var prompt = _promptBuilder.Build(metadata);
        var rawOutput = await _aiClient.GenerateAsync(prompt);
        return ParseOutput(rawOutput);
    }

    private static CommercialReport ParseOutput(string rawOutput)
    {
        var result = DeserializeAiResult(rawOutput);

        return new CommercialReport
        {
            Title = string.Empty,
            PatentOwner = string.Empty,
            PatentStatus = string.Empty,
            TechnologyTags = result.TechnologyTags ?? new List<string>(),
            PotentialMarkets = result.PotentialMarkets ?? new List<string>(),
            CommercialOpportunities = result.CommercialOpportunities ?? new List<string>()
        };
    }

    private static AiAnalysisResult DeserializeAiResult(string rawOutput)
    {
        var json = ExtractJson(rawOutput);
        if (string.IsNullOrWhiteSpace(json))
            return new AiAnalysisResult();

        try
        {
            var result = JsonSerializer.Deserialize<AiAnalysisResult>(json);
            return result ?? new AiAnalysisResult();
        }
        catch
        {
            return new AiAnalysisResult();
        }
    }

    private static string ExtractJson(string rawOutput)
    {
        var trimmed = rawOutput.Trim();
        var match = Regex.Match(trimmed, @"\{[\s\S]*\}");
        return match.Success ? match.Value : trimmed;
    }
}
