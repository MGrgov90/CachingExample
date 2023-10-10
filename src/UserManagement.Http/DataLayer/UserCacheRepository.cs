using UserManagement.Http.Controllers;
using UserManagement.Http.Domain;
using UserManagement.Http.Infrastructure;

namespace UserManagement.Http.DataLayer;

public class UserCacheRepository : IUserRepository
{
    private readonly IRedisCache _cache;
    private readonly IUserRepository _userRepository;

    public UserCacheRepository(IRedisCache redisCache, IUserRepository userRepository)
    {
        _cache = redisCache;
        _userRepository = userRepository;

    }

    public IQueryable<UserAddress> GetByAddress(string street, string number)
    {
        var delimiter = "_";
        var key = $"{street}{delimiter}{number}";
        return _cache.GetAndSetAsync(key,
            () => _userRepository.GetByAddress(street, number));
    }

    public User GetByEmail(string email)
    {
        return _cache.GetAndSetAsync(email,
            () => _userRepository.GetByEmail(email));
    }

    public void Update(UserUpdateRequest request)
    {
        _cache.RemoveKey(request.Email);
        _userRepository.Update(request);
    }
}

