using System.Text.Json;
using PCBack.Models;

namespace PCBack.Services;

public class PatentService : IPatentService
{
    private const string PatentsViewQueryUrl = "https://api.patentsview.org/patents/query";
    private const int PatentTermYears = 20;

    private readonly HttpClient _httpClient;

    public PatentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PatentMetadata?> GetPatentMetadataAsync(string patentNumber)
    {
        if (string.IsNullOrWhiteSpace(patentNumber))
            return null;

        var request = new PatentsViewRequest
        {
            Q = new PatentsViewQuery { PatentNumber = patentNumber.Trim() }
        };

        using var response = await _httpClient.PostAsJsonAsync(PatentsViewQueryUrl, request);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<PatentsViewResponse>(json);

        if (apiResponse?.Patents is not { Count: > 0 })
            return null;

        var patent = apiResponse.Patents[0];
        var status = ComputePatentStatus(patent.PatentDate);

        var owner = patent.Assignees?
            .Where(a => !string.IsNullOrWhiteSpace(a.AssigneeOrganization))
            .Select(a => a.AssigneeOrganization!)
            .FirstOrDefault();

        return new PatentMetadata
        {
            Title = patent.PatentTitle,
            Abstract = patent.PatentAbstract,
            PatentOwner = owner,
            PatentStatus = status
        };
    }

    private static string ComputePatentStatus(string? patentDate)
    {
        if (string.IsNullOrWhiteSpace(patentDate))
            return "Active";

        if (!DateTime.TryParse(patentDate, out var date))
            return "Active";

        var expiryDate = date.AddYears(PatentTermYears);
        return DateTime.UtcNow.Date > expiryDate ? "Expired" : "Active";
    }
}
