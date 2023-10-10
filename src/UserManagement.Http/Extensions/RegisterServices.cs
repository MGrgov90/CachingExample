using Microsoft.EntityFrameworkCore;
using UserManagement.Http.DataLayer;

namespace UserManagement.Http.Extensions;

public static class RegisterServices
{
    public static void RegisterDbContext(this IServiceCollection services)
    {
        services.AddDbContext<UserManagementContext>(o => o.UseInMemoryDatabase("UserDatabase"));
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void RegisterRedis(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "testInstance_";
        });
    }
}
