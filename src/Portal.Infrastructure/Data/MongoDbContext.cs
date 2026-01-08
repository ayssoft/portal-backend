using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Portal.Core.Entities;
using Portal.Domain.Attributes;

namespace Portal.Infrastructure.Data;

/// <summary>
/// MongoDB context for database connection and collection management
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    /// <summary>
    /// Get MongoDB collection for a specific entity type
    /// </summary>
    public IMongoCollection<T> GetCollection<T>() where T : BaseEntity
    {
        var collectionName = GetCollectionName<T>();
        return _database.GetCollection<T>(collectionName);
    }

    /// <summary>
    /// Get collection name from BsonCollectionAttribute or use type name
    /// </summary>
    private static string GetCollectionName<T>()
    {
        var attribute = (BsonCollectionAttribute?)Attribute.GetCustomAttribute(
            typeof(T), typeof(BsonCollectionAttribute));

        return attribute?.CollectionName ?? typeof(T).Name.ToLowerInvariant();
    }
}
