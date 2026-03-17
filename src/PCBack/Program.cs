using PCBack.AI;
using PCBack.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
