namespace UserManagement.Http.Infrastructure;

public interface IRedisCache
{
    T GetAndSetAsync<T>(string key,
        Func<T> getWithoutCache);

    void RemoveKey(string redisKey);
}

