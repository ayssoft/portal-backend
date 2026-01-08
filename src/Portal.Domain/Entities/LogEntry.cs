using MongoDB.Bson.Serialization.Attributes;
using Portal.Core.Entities;
using Portal.Domain.Attributes;

namespace Portal.Domain.Entities;

/// <summary>
/// Log entry entity for application logging
/// </summary>
[BsonCollection("logs")]
public class LogEntry : BaseEntity
{
    [BsonElement("level")]
    public string Level { get; set; } = string.Empty;

    [BsonElement("message")]
    public string Message { get; set; } = string.Empty;

    [BsonElement("exception")]
    public string? Exception { get; set; }

    [BsonElement("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [BsonElement("userId")]
    public string? UserId { get; set; }

    [BsonElement("requestPath")]
    public string? RequestPath { get; set; }

    [BsonElement("requestMethod")]
    public string? RequestMethod { get; set; }

    [BsonElement("statusCode")]
    public int? StatusCode { get; set; }
}
