using Microsoft.EntityFrameworkCore;
using UserManagement.Http.Domain;

namespace UserManagement.Http.DataLayer;


public class UserManagementContext : DbContext
{
    public UserManagementContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<Address>().HasKey(x => x.Id);
        modelBuilder.Entity<UserAddress>().HasKey(x => x.Id);

        //modelBuilder.Entity<User>().HasMany(x => x.UserAddresses)
        //    .WithOne(x => x.User)
        //    .HasForeignKey(x => x.UserId);
        //modelBuilder.Entity<Address>().HasMany(x => x.UserAddresses)
        //    .WithOne(x => x.Address)
        //    .HasForeignKey(x => x.AddressId);

        modelBuilder.Entity<User>()
        .HasMany(x => x.Addresses)
        .WithMany(x => x.Users)
        .UsingEntity<UserAddress>(
            x => x.HasOne<Address>().WithMany().HasForeignKey(e => e.AddressId),
            x => x.HasOne<User>().WithMany().HasForeignKey(e => e.UserId));

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
}
