using System.Linq.Expressions;
using MongoDB.Driver;
using Portal.Core.Entities;
using Portal.Core.Interfaces;
using Portal.Infrastructure.Data;

namespace Portal.Infrastructure.Repositories;

/// <summary>
/// Generic MongoDB repository implementation
/// </summary>
/// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
public class MongoRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<T>();
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(x => x.Id, id),
            Builders<T>.Filter.Eq(x => x.IsDeleted, false)
        );

        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Where(predicate),
            Builders<T>.Filter.Eq(x => x.IsDeleted, false)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = null;
        entity.IsDeleted = false;

        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(x => x.Id, id),
            Builders<T>.Filter.Eq(x => x.IsDeleted, false)
        );

        var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> SoftDeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var update = Builders<T>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var filter = predicate != null
            ? Builders<T>.Filter.And(
                Builders<T>.Filter.Where(predicate),
                Builders<T>.Filter.Eq(x => x.IsDeleted, false)
            )
            : Builders<T>.Filter.Eq(x => x.IsDeleted, false);

        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }
}
