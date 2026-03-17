using PCBack.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Service layer
builder.Services.AddScoped<IPatentService, PatentService>();
builder.Services.AddScoped<IAiAnalysisService, AiAnalysisService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
