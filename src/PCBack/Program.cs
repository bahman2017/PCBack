using Microsoft.EntityFrameworkCore;
using PCBack.AI;
using PCBack.Data;
using PCBack.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("Testing"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddControllers();

// AI pipeline
builder.Services.AddSingleton<PromptBuilder>();
builder.Services.AddHttpClient<IAiClient, AiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
});

// Service layer
builder.Services.AddHttpClient<IPatentService, PatentService>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler { UseCookies = false });
builder.Services.AddScoped<IAiAnalysisService, AiAnalysisService>();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
