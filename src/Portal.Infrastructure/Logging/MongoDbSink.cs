using MongoDB.Driver;
using Portal.Domain.Entities;
using Portal.Infrastructure.Data;
using Serilog.Core;
using Serilog.Events;

namespace Portal.Infrastructure.Logging;

/// <summary>
/// Custom Serilog sink for MongoDB logging
/// </summary>
public class MongoDbSink : ILogEventSink
{
    private readonly IMongoCollection<LogEntry> _collection;

    public MongoDbSink(MongoDbContext context)
    {
        _collection = context.GetCollection<LogEntry>();
    }

    public void Emit(LogEvent logEvent)
    {
        var logEntry = new LogEntry
        {
            Level = logEvent.Level.ToString(),
            Message = logEvent.RenderMessage(),
            Exception = logEvent.Exception?.ToString(),
            Timestamp = logEvent.Timestamp.UtcDateTime,
            Properties = logEvent.Properties.ToDictionary(
                p => p.Key,
                p => (object)p.Value.ToString()
            )
        };

        // Extract additional properties if available
        if (logEvent.Properties.TryGetValue("UserId", out var userId))
        {
            logEntry.UserId = userId.ToString().Trim('"');
        }

        if (logEvent.Properties.TryGetValue("RequestPath", out var requestPath))
        {
            logEntry.RequestPath = requestPath.ToString().Trim('"');
        }

        if (logEvent.Properties.TryGetValue("RequestMethod", out var requestMethod))
        {
            logEntry.RequestMethod = requestMethod.ToString().Trim('"');
        }

        if (logEvent.Properties.TryGetValue("StatusCode", out var statusCode))
        {
            var statusCodeStr = statusCode.ToString().Trim('"');
            if (int.TryParse(statusCodeStr, out var code))
            {
                logEntry.StatusCode = code;
            }
        }

        // Use fire-and-forget pattern to avoid blocking
        Task.Run(async () => await _collection.InsertOneAsync(logEntry));
    }
}
