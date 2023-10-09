using Microsoft.EntityFrameworkCore;
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
        Task.Delay(new Random().Next(500, 2000)).Wait();

        var user = _context.Users
            .Include(x => x.UserAddresses)
            .ThenInclude(x => x.Address)
            .First(x => x.Email == email);

        return user;
    }
}

