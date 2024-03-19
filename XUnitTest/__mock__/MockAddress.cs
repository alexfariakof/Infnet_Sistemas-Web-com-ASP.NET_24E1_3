using Domain.Account.ValueObject;
using Bogus;
using Application.Account.Dto;

namespace __mock__;
public class MockAddress
{
    private static readonly Lazy<MockAddress> _instance = new Lazy<MockAddress>(() => new MockAddress());

    public static MockAddress Instance => _instance.Value;

    private MockAddress() { }

    public Address GetFaker()
    {
        var fakeAddress = new Faker<Address>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(a => a.Zipcode, f => f.Address.ZipCode())
            .RuleFor(a => a.Street, f => f.Address.StreetName())
            .RuleFor(a => a.Number, f => f.Address.BuildingNumber())
            .RuleFor(a => a.Neighborhood, f => f.Address.SecondaryAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(a => a.Complement, f => f.Address.SecondaryAddress())
            .RuleFor(a => a.Country, f => f.Address.Country())
            .Generate();

        return fakeAddress;
    }

    public List<Address> GetListFaker(int count)
    {
        var addressList = new List<Address>();
        for (var i = 0; i < count; i++)
        {
            addressList.Add(GetFaker());
        }
        return addressList;
    }

    public AddressDto GetDtoFromAddress(Address address)
    {
        var addressDto = new AddressDto
        {
            Zipcode = address.Zipcode,
            Street = address.Street,
            Number = address.Number,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State,
            Complement = address.Complement,
            Country = address.Country
        };

        return addressDto;
    }
}
