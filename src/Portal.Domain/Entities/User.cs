using MongoDB.Bson.Serialization.Attributes;
using Portal.Core.Entities;
using Portal.Domain.Attributes;

namespace Portal.Domain.Entities;

/// <summary>
/// User entity representing application users
/// </summary>
[BsonCollection("users")]
public class User : BaseEntity
{
    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
}
