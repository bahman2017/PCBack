using PCBack.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Service layer
builder.Services.AddHttpClient<IPatentService, PatentService>();
builder.Services.AddScoped<IAiAnalysisService, AiAnalysisService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
