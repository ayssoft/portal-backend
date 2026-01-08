using Microsoft.Extensions.Caching.Memory;

namespace Portal.Infrastructure.Caching;

/// <summary>
/// Memory cache service for performance optimization
/// </summary>
public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _defaultOptions;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
        _defaultOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            SlidingExpiration = TimeSpan.FromMinutes(10)
        };
    }

    /// <summary>
    /// Get cached item by key
    /// </summary>
    public T? Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    /// <summary>
    /// Set cache item with default options
    /// </summary>
    public void Set<T>(string key, T value)
    {
        _cache.Set(key, value, _defaultOptions);
    }

    /// <summary>
    /// Set cache item with custom expiration
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration
        };
        _cache.Set(key, value, options);
    }

    /// <summary>
    /// Get or create cache item
    /// </summary>
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
    {
        if (_cache.TryGetValue(key, out T? cachedValue) && cachedValue != null)
        {
            return cachedValue;
        }

        var value = await factory();
        Set(key, value);
        return value;
    }

    /// <summary>
    /// Remove item from cache
    /// </summary>
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// Check if key exists in cache
    /// </summary>
    public bool Exists(string key)
    {
        return _cache.TryGetValue(key, out _);
    }
}
