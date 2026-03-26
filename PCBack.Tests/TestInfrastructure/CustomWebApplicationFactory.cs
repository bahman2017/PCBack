using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCBack.Data;
using PCBack.Services;

namespace PCBack.Tests.TestInfrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<PCBack.Program>
{
    private readonly string _inMemoryDatabaseName = Guid.NewGuid().ToString("N");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Run after Program.cs so Npgsql DbContext registration is removed cleanly.
        builder.ConfigureTestServices(services =>
        {
            foreach (var d in services.Where(x =>
                         x.ServiceType == typeof(ApplicationDbContext) ||
                         x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)).ToList())
            {
                services.Remove(d);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_inMemoryDatabaseName));

            foreach (var d in services.Where(x => x.ServiceType == typeof(IAiAnalysisService)).ToList())
                services.Remove(d);

            services.AddScoped<IAiAnalysisService, Fakes.FakeAiAnalysisService>();
        });
    }
}
