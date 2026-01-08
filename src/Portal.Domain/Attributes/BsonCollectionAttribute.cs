namespace Portal.Domain.Attributes;

/// <summary>
/// Attribute to specify MongoDB collection name for an entity
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
