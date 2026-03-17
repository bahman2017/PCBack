namespace PCBack.AI;

/// <summary>
/// Base prompt templates for patent commercialization analysis.
/// </summary>
public static class PromptTemplates
{
    public const string CommercializationAnalysis = """
        You are a technology commercialization expert.

        Analyze the following patent and provide structured insights.

        Patent Title:
        {title}

        Patent Abstract:
        {abstract}

        Return the result strictly as JSON in this format. Do not include explanations outside the JSON.

        {
          "technologyTags": ["tag1","tag2"],
          "potentialMarkets": ["market1","market2"],
          "commercialOpportunities": ["opportunity1","opportunity2"]
        }

        Use technologyTags for technology category and relevant domains.
        Use potentialMarkets for possible industries and markets.
        Use commercialOpportunities for startup opportunities, licensing strategies, and product ideas.
        """;
}
