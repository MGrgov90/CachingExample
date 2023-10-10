using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UserManagement.Http.Infrastructure;

public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;

    public RedisCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T GetAndSetAsync<T>(string key,
        Func<T> getWithoutCache)
    {
        var cacheResult = _cache.Get(key);
        if (cacheResult != null)
            return JsonSerializer.Deserialize<T>(cacheResult);

        var data = getWithoutCache();
        _cache.SetStringAsync(key,
            JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }));

        return data;
    }

    public void RemoveKey(string redisKey)
    {
        _cache.Remove(redisKey);
    }
}

