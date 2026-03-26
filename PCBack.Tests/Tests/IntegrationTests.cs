using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PCBack.Data;
using PCBack.Models;
using PCBack.Tests.TestInfrastructure;

namespace PCBack.Tests;

public class IntegrationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task Analyze_Should_Return_Report_And_Save_To_DB()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var request = new PatentAnalysisRequest
        {
            Abstract = "Test battery innovation for EV efficiency"
        };

        var response = await client.PostAsJsonAsync("/api/patents/analyze", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CommercialReport>(JsonOptions);

        Assert.NotNull(result);
        Assert.NotNull(result.TechnologyTags);
        Assert.Contains("AI", result.TechnologyTags);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var saved = db.PatentAnalyses.ToList();

        Assert.Single(saved);
    }

    [Fact]
    public async Task History_Should_Return_Persisted_Analysis()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var request = new PatentAnalysisRequest { Abstract = "Integration history test" };
        var post = await client.PostAsJsonAsync("/api/patents/analyze", request);
        post.EnsureSuccessStatusCode();

        var historyResponse = await client.GetAsync("/api/patents/history");
        historyResponse.EnsureSuccessStatusCode();

        var items = await historyResponse.Content.ReadFromJsonAsync<List<PatentAnalysisHistoryItem>>(JsonOptions);

        Assert.NotNull(items);
        Assert.Single(items);
        Assert.Contains("AI", items[0].TechnologyTags);
    }
}
