using UserManagement.Http.Domain;

namespace UserManagement.Http.Response;

public class UserFilterByAddressResponse
{
    public AddressType AddressType { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public string Email { get; set; }
}

