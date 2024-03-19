using AutoMapper;
using Application.Account.Dto;
using Domain.Account.ValueObject;
using Application.Account.Profile;

namespace Application.Account;
public class AddressProfileTest
{
    [Fact]
    public void Map_AddressDto_To_Address_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AddressProfile>();
        }));

        var addressDto = new AddressDto
        {
            Zipcode = "12345-678",
            Street = "Main Street",
            Number = "123",
            Neighborhood = "Downtown",
            City = "Cityville",
            State = "ST",
            Complement = "Apt 456",
            Country = "Countryland"
        };

        // Act
        var address = mapper.Map<Address>(addressDto);

        // Assert
        Assert.NotNull(address);
        Assert.Equal(addressDto.Zipcode, address.Zipcode);
        Assert.Equal(addressDto.Street, address.Street);
        Assert.Equal(addressDto.Number, address.Number);
        Assert.Equal(addressDto.Neighborhood, address.Neighborhood);
        Assert.Equal(addressDto.City, address.City);
        Assert.Equal(addressDto.State, address.State);
        Assert.Equal(addressDto.Complement, address.Complement);
        Assert.Equal(addressDto.Country, address.Country);
    }

    [Fact]
    public void Map_Address_To_AddressDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AddressProfile>();
        }));

        var address = new Address
        {
            Zipcode = "12345-678",
            Street = "Main Street",
            Number = "123",
            Neighborhood = "Downtown",
            City = "Cityville",
            State = "ST",
            Complement = "Apt 456",
            Country = "Countryland"
        };

        // Act
        var addressDto = mapper.Map<AddressDto>(address);

        // Assert
        Assert.NotNull(addressDto);
        Assert.Equal(address.Zipcode, addressDto.Zipcode);
        Assert.Equal(address.Street, addressDto.Street);
        Assert.Equal(address.Number, addressDto.Number);
        Assert.Equal(address.Neighborhood, addressDto.Neighborhood);
        Assert.Equal(address.City, addressDto.City);
        Assert.Equal(address.State, addressDto.State);
        Assert.Equal(address.Complement, addressDto.Complement);
        Assert.Equal(address.Country, addressDto.Country);
    }
}
