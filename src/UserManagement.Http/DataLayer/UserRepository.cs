using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserManagement.Http.Domain;

namespace UserManagement.Http.DataLayer;

public interface IUserRepository
{
    IQueryable<UserAddress> GetByAddress(string street, string number);
    User GetByEmail(string email);
}

public class UserRepository : IUserRepository
{
    private readonly UserManagementContext _context;
    private readonly IDistributedCache _cache;

    public UserRepository(UserManagementContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IQueryable<UserAddress> GetByAddress(string street, string number)
    {
        Task.Delay(new Random().Next(500, 2000)).Wait();

        return _context.UserAddresses.Include(x => x.User)
            .Where(x => x.Address.Street == street && x.Address.Number == number);
    }

    public User GetByEmail(string email)
    {
        var val = _cache.Get(email);
        if (val != null)
            return JsonSerializer.Deserialize<User>(val);

        Task.Delay(new Random().Next(500, 2000)).Wait();

        var user = _context.Users
            .Include(x => x.UserAddresses)
            .ThenInclude(x => x.Address)
            .First(x => x.Email == email);


        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }));
        _cache.Set(email, bytes);

        return user;
    }
}

