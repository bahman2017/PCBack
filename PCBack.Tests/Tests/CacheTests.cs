using Microsoft.EntityFrameworkCore;
using PCBack.Data;
using PCBack.Models;
using PCBack.Tests.Fakes;

namespace PCBack.Tests;

public class CacheTests
{
    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Should_Return_Cached_Result_When_PatentNumber_Exists()
    {
        await using var db = CreateDbContext();

        db.PatentAnalyses.Add(new PatentAnalysis
        {
            Id = Guid.NewGuid(),
            PatentNumber = "123",
            Title = "Cached",
            PatentOwner = "Owner",
            PatentStatus = "Active",
            TechnologyTags = "AI",
            PotentialMarkets = "Healthcare",
            CommercialOpportunities = "Licensing",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();

        var service = new AiAnalysisServiceFake(db);

        var result = await service.GenerateAsync("123", null);

        Assert.NotNull(result);
        Assert.Contains("AI", result.TechnologyTags);
        Assert.True(service.CacheHit);
    }

    [Fact]
    public async Task Should_Call_AI_And_Save_When_No_Cache()
    {
        await using var db = CreateDbContext();

        var service = new AiAnalysisServiceFake(db);

        var result = await service.GenerateAsync("999", "new abstract");

        Assert.NotNull(result);
        Assert.False(service.CacheHit);

        var saved = db.PatentAnalyses.FirstOrDefault(x => x.PatentNumber == "999");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task Should_Return_History_Items_From_DB()
    {
        await using var db = CreateDbContext();

        db.PatentAnalyses.Add(new PatentAnalysis
        {
            Id = Guid.NewGuid(),
            PatentNumber = "111",
            Title = "T",
            PatentOwner = "O",
            PatentStatus = "Active",
            TechnologyTags = "Energy",
            PotentialMarkets = "EV",
            CommercialOpportunities = "Startup",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();

        var items = db.PatentAnalyses
            .OrderByDescending(x => x.CreatedAt)
            .Take(20)
            .ToList();

        Assert.Single(items);
        Assert.Equal("111", items[0].PatentNumber);
    }
}
