namespace UserManagement.Http.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public string Email { get; set; }

    public ICollection<UserAddress> UserAddresses { get; set; }
    public ICollection<Address> Addresses { get; set; }

}
