using UserManagement.Http.Domain;

namespace UserManagement.Http.Response;

public class UserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public string Email { get; set; }
    public Dictionary<AddressType, IEnumerable<UserAddressResponse>> Addresses { get; set; }
}

public class UserAddressResponse
{
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
}

