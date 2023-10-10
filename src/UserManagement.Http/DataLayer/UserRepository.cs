using Microsoft.EntityFrameworkCore;
using UserManagement.Http.Controllers;
using UserManagement.Http.Domain;

namespace UserManagement.Http.DataLayer;

public interface IUserRepository
{
    IQueryable<UserAddress> GetByAddress(string street, string number);
    User GetByEmail(string email);
    void Update(UserUpdateRequest request);
}

public class UserRepository : IUserRepository
{
    private readonly UserManagementContext _context;

    public UserRepository(UserManagementContext context)
    {
        _context = context;
    }

    public IQueryable<UserAddress> GetByAddress(string street, string number)
    {
        Task.Delay(new Random().Next(500, 2000)).Wait();

        return _context.UserAddresses.Include(x => x.User)
            .Where(x => x.Address.Street == street && x.Address.Number == number);
    }

    public User GetByEmail(string email)
    {
        //var val = _cache.Get(email);
        //if (val != null)
        //    return JsonSerializer.Deserialize<User>(val);

        Task.Delay(new Random().Next(500, 2000)).Wait();

        var user = _context.Users
            .Include(x => x.UserAddresses)
            .ThenInclude(x => x.Address)
            .First(x => x.Email == email);


        //var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }));
        //_cache.Set(email, bytes);
        //_cache.Set(email,
        //    bytes,
        //    new DistributedCacheEntryOptions()
        //    {
        //        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(90),
        //        SlidingExpiration = TimeSpan.FromSeconds(45)
        //    });

        return user;
    }

    public void Update(UserUpdateRequest request)
    {
        //var val = _cache.Get(request.Email);
        //if (val != null)
        //{
        //    _cache.Remove(request.Email);
        //}

        var user = _context.Users
            .First(x => x.Email == request.Email);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        _context.SaveChanges();

    }
}

