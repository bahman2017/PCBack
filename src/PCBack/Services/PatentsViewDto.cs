using System.Text.Json.Serialization;

namespace PCBack.Services;

/// <summary>
/// Internal DTOs for PatentsView PatentSearch API request and response.
/// </summary>

internal sealed class PatentsViewRequest
{
    [JsonPropertyName("filter")]
    public PatentsViewFilter Filter { get; set; } = new();

    [JsonPropertyName("fields")]
    public string[] Fields { get; set; } =
    {
        "patent_number",
        "patent_title",
        "patent_abstract",
        "assignee_organization",
        "patent_date"
    };
}

internal sealed class PatentsViewFilter
{
    [JsonPropertyName("patent_number")]
    public string[] PatentNumber { get; set; } = Array.Empty<string>();
}

internal sealed class PatentsViewResponse
{
    [JsonPropertyName("data")]
    public List<PatentsViewPatent>? Data { get; set; }
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
