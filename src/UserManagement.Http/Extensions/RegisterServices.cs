using Microsoft.EntityFrameworkCore;
using UserManagement.Http.DataLayer;
using UserManagement.Http.Infrastructure;

namespace UserManagement.Http.Extensions;

public static class RegisterServices
{
    public static void RegisterDbContext(this IServiceCollection services)
    {
        services.AddDbContext<UserManagementContext>(o => o.UseInMemoryDatabase("UserDatabase"));
    }

    public static void RegisterRedis(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "testInstance_";
        });

        services.AddSingleton<IRedisCache, RedisCache>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();

        services.AddScoped<IUserRepository>(svcProvider =>
            new UserCacheRepository(svcProvider.GetRequiredService<IRedisCache>(), svcProvider.GetRequiredService<UserRepository>()));
    }
}
