using UserManagement.Http.DataLayer;
using UserManagement.Http.Domain;

namespace UserManagement.Http.Extensions;

public static class DbInitializer
{
    public static IApplicationBuilder SeedInMemoryData(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<UserManagementContext>();
            Seed(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<UserManagementContext>>();
            logger.LogCritical(ex, ex.Message);

            var life = services.GetRequiredService<IHostApplicationLifetime>();
            life.ApplicationStopped.Register(() =>
            {
                logger.LogInformation("Application is stopped");
            });
            life.StopApplication();
        }

        return app;
    }

    private static void Seed(UserManagementContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        context.Database.EnsureCreated();
        if (context.Users.Any()) return;

        var officeAddresses = GetOfficeAddresses();
        context.AddRange(officeAddresses);
        var personalAddresses = GetPersonalAddresses();
        context.AddRange(personalAddresses);
        var users = GetUsers();
        context.AddRange(users);

        foreach (var user in users)
        {
            var officeIndex = new Random().Next(0, officeAddresses.Count());
            var personalIndex = new Random().Next(0, personalAddresses.Count());

            var officeAddress = new UserAddress()
            {
                Address = officeAddresses.ElementAt(officeIndex),
                AddressType = AddressType.Work,
                User = user,
                Id = Guid.NewGuid(),
            };
            var personalAddress = new UserAddress()
            {
                Address = personalAddresses.ElementAt(personalIndex),
                AddressType = AddressType.Home,
                User = user,
                Id = Guid.NewGuid(),
            };

            context.Add(personalAddress);
            context.Add(officeAddress);
        }
        foreach (var personalAddress in personalAddresses)
        {
            var userIndex = new Random().Next(0, users.Count());

            var officeAddress = new UserAddress()
            {
                Address = personalAddress,
                AddressType = AddressType.Work,
                User = users.ElementAt(userIndex),
                Id = Guid.NewGuid(),
            };

            context.Add(officeAddress);
        }

        context.SaveChanges();
    }

    private static IEnumerable<Address> GetOfficeAddresses()
    {
        var htecAddress = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Bulevar Madijana",
            Number = "10",
        };

        var traceOneAddress = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Bulevar Svetog cara Konstantina",
            Number = "80",
        };

        var badinAddress = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Nikole Pasica",
            Number = "28",
        };

        return new[] { htecAddress, traceOneAddress, badinAddress };
    }

    private static IEnumerable<Address> GetPersonalAddresses()
    {
        var a1 = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Vizantijski Bulevar",
            Number = "100",
        };

        var a2 = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Tome Rosandica",
            Number = "22",
        };

        var a3 = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Milojka Lesjanina",
            Number = "51",
        };

        var a4 = new Address()
        {
            State = "Serbia",
            City = "Nis",
            Street = "Bulevar Nemanjica",
            Number = "103",
        };

        return new[] { a1, a2, a3, a4, };
    }

    private static IEnumerable<User> GetUsers()
    {
        var users = GenFu.GenFu.ListOf<User>(15);

        users[1].Email = "mgrgov@gmail.com";

        return users;
    }

}
