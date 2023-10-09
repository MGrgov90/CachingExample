namespace UserManagement.Http.Domain;

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }

    public ICollection<UserAddress> UserAddresses { get; set; }
    public ICollection<User> Users { get; set; }

}
