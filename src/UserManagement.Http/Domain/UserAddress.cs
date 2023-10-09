namespace UserManagement.Http.Domain;

public class UserAddress
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }


    public User User { get; set; }
    public Address Address { get; set; }
    public AddressType AddressType { get; set; }
}
