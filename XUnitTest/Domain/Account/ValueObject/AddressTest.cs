using Domain.Account.ValueObject;

namespace Domain.Account.Tests;
public class AddressTest
{
    [Fact]
    public void Should_Set_Properties_Correctly()
    {
        // Arrange
        var mockAddress = MockAddress.Instance.GetFaker();

        // Act
        var address = new Address
        {
            Zipcode = mockAddress.Zipcode,
            Street = mockAddress.Street,
            Number = mockAddress.Number,
            Neighborhood = mockAddress.Neighborhood,
            City = mockAddress.City,
            State = mockAddress.State,
            Complement = mockAddress.Complement,
            Country = mockAddress.Country
        };

        // Assert
        Assert.Equal(mockAddress.Zipcode, address.Zipcode);
        Assert.Equal(mockAddress.Street, address.Street);
        Assert.Equal(mockAddress.Number, address.Number);
        Assert.Equal(mockAddress.Neighborhood, address.Neighborhood);
        Assert.Equal(mockAddress.City, address.City);
        Assert.Equal(mockAddress.State, address.State);
        Assert.Equal(mockAddress.Complement, address.Complement);
        Assert.Equal(mockAddress.Country, address.Country);
    }

    [Fact]
    public void Should_Allow_Nullable_Properties()
    {
        // Arrange
        var address = new Address();

        // Act & Assert
        Assert.Null(address.Zipcode);
        Assert.Null(address.Street);
        Assert.Null(address.Number);
        Assert.Null(address.Neighborhood);
        Assert.Null(address.City);
        Assert.Null(address.State);
        Assert.Null(address.Complement);
        Assert.Null(address.Country);
    }
}
