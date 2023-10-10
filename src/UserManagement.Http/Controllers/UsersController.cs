using Microsoft.AspNetCore.Mvc;
using UserManagement.Http.DataLayer;
using UserManagement.Http.Domain;
using UserManagement.Http.Response;

namespace UserManagement.Http.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet("{email}")]
    public UserResponse Get(string email)
    {
        User user = _userRepository.GetByEmail(email)!;
        var addresses = user.UserAddresses.ToDictionary(x => x.AddressType);

        return new UserResponse()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Addresses = GetAddresses(user)
        };
    }

    [HttpPut]
    public void Put([FromBody] UserUpdateRequest request)
    {
        _userRepository.Update(request);
    }

    [HttpGet("byAddress")]
    public IEnumerable<UserFilterByAddressResponse> Get([FromQuery] string street,
        [FromQuery] string number)
    {
        var userAddresses = _userRepository.GetByAddress(street, number).ToList();

        return userAddresses.Select(x => new UserFilterByAddressResponse()
        {
            Email = x.User.Email,
            FirstName = x.User.FirstName,
            LastName = x.User.LastName,
            AddressType = x.AddressType,
        }).ToList();
    }

    private Dictionary<AddressType, IEnumerable<UserAddressResponse>> GetAddresses(User user)
    {
        Dictionary<AddressType, IEnumerable<UserAddressResponse>> result = new();

        foreach (var address in user.UserAddresses)
        {
            var addressDto = new UserAddressResponse()
            {
                City = address.Address.City,
                State = address.Address.State,
                Street = address.Address.Street,
                Number = address.Address.Number
            };

            if (result.ContainsKey(address.AddressType))
            {
                result[address.AddressType].Append(addressDto);
            }
            else
            {
                result.Add(address.AddressType, new[] { addressDto });
            }
        }

        return result;
    }
}

public class UserUpdateRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}