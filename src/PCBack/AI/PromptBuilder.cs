using PCBack.Models;

namespace PCBack.AI;

/// <summary>
/// Builds the prompt for patent commercialization analysis from patent metadata.
/// </summary>
public class PromptBuilder
{
    public string Build(PatentMetadata metadata)
    {
        if (metadata == null)
            return string.Empty;

        var title = metadata.Title ?? "(No title provided)";
        var abstractText = metadata.Abstract ?? "(No abstract provided)";

        return PromptTemplates.CommercializationAnalysis
            .Replace("{title}", title)
            .Replace("{abstract}", abstractText);
    }
}
