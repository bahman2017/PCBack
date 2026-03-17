using System.Text.Json.Serialization;

namespace PCBack.Services;

/// <summary>
/// Internal DTOs for PatentsView API request and response.
/// </summary>

internal sealed class PatentsViewRequest
{
    [JsonPropertyName("q")]
    public PatentsViewQuery Q { get; set; } = new();

    [JsonPropertyName("f")]
    public string[] F { get; set; } =
    {
        "patent_title",
        "patent_abstract",
        "assignee_organization",
        "patent_date"
    };
}

internal sealed class PatentsViewQuery
{
    [JsonPropertyName("patent_number")]
    public string PatentNumber { get; set; } = string.Empty;
}

internal sealed class PatentsViewResponse
{
    [JsonPropertyName("patents")]
    public List<PatentsViewPatent>? Patents { get; set; }
}

internal sealed class PatentsViewPatent
{
    [JsonPropertyName("patent_title")]
    public string? PatentTitle { get; set; }

    [JsonPropertyName("patent_abstract")]
    public string? PatentAbstract { get; set; }

    [JsonPropertyName("assignees")]
    public List<PatentsViewAssignee>? Assignees { get; set; }

    [JsonPropertyName("patent_date")]
    public string? PatentDate { get; set; }
}

internal sealed class PatentsViewAssignee
{
    [JsonPropertyName("assignee_organization")]
    public string? AssigneeOrganization { get; set; }
}
