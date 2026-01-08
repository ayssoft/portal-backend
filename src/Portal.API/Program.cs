using Portal.Application.Services;
using Portal.Core.Interfaces;
using Portal.Domain.Entities;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Data;
using Portal.Infrastructure.Logging;
using Portal.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/portal-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();

// Register MongoDB Sink for Serilog
builder.Services.AddSingleton<MongoDbSink>();

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Register cache service
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheService>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Portal API",
        Version = "v1",
        Description = "N-Layered Architecture API with MongoDB and Serilog"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portal API v1");
    });
}

// Add Serilog request logging
app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting Portal API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
