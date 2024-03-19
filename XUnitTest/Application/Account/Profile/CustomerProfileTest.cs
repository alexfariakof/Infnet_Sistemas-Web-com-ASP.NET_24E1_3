using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Account.Agreggates;
using Application.Transactions.Dto;

namespace Application.Account;
public class CustomerProfileTest
{

    [Fact]
    public void Map_CustomerDto_To_Customer_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<CustomerProfile>();
        }));

        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",            
            CPF = "12345678901",
            Birth = new DateTime(1990, 1, 1),
            Phone = "123456789",
            Address = new AddressDto
            {
                Zipcode = "12345-678",
                Street = "Main Street",
                Number = "123",
                Neighborhood = "Downtown",
                City = "Cityville",
                State = "ST",
                Complement = "Apt 456",
                Country = "Countryland"
            },
            FlatId = Guid.NewGuid(),
            Card = new CardDto
            {
                Limit = 1000,
                Number = "1234567812345678",
                Validate = DateTime.Now.AddYears(1),
                CVV = "123"
            }
        };

        // Act
        var customer = mapper.Map<Customer>(customerDto);

        // Assert
        Assert.NotNull(customer);
        Assert.Equal(customerDto.Id, customer.Id);
        Assert.Equal(customerDto.Name, customer.Name);
        Assert.Equal(customerDto.Email, customer.User.Login.Email);
        Assert.Equal(customerDto.CPF, customer.CPF);
        Assert.Equal(customerDto.Birth, customer.Birth);
        Assert.NotNull(customer.Phone);
        Assert.Equal(customerDto.Phone, customer.Phone?.Number);
        Assert.NotNull(customer.Addresses);
        Assert.NotNull(customer.Cards);
    }

    [Fact]
    public void Map_Customer_To_CustomerDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<CustomerProfile>();
        }));

        var customer = MockCustomer.Instance.GetFaker();
        customer.AddAdress(MockAddress.Instance.GetFaker());        
        customer.AddCard(MockCard.Instance.GetFaker());
        customer.AddFlat(customer, MockFlat.Instance.GetFaker(), customer.Cards.FirstOrDefault());

        // Act
        var customerDto = mapper.Map<CustomerDto>(customer);

        // Assert
        Assert.NotNull(customerDto);
        Assert.Equal(customer.Id, customerDto.Id);
        Assert.Equal(customer.Name, customerDto.Name);
        Assert.Equal(customer.User.Login.Email, customerDto.Email);
        Assert.Equal(customer.CPF, customerDto.CPF);
        Assert.Equal(customer.Birth, customerDto.Birth);
        Assert.NotNull(customerDto.Phone);
        Assert.Equal(customer.Phone?.Number, customerDto.Phone);
        Assert.NotNull(customerDto.Address);
        Assert.Equal(customer.Addresses.First().Zipcode, customerDto.Address.Zipcode);
        Assert.Equal(customer.Addresses.First().Street, customerDto.Address.Street);
        Assert.Equal(customer.Addresses.First().Number, customerDto.Address.Number);
        Assert.Equal(customer.Addresses.First().Neighborhood, customerDto.Address.Neighborhood);
        Assert.Equal(customer.Addresses.First().City, customerDto.Address.City);
        Assert.Equal(customer.Addresses.First().State, customerDto.Address.State);
        Assert.Equal(customer.Addresses.First().Complement, customerDto.Address.Complement);
        Assert.Equal(customer.Addresses.First().Country, customerDto.Address.Country);
        Assert.Null(customerDto.Card);
    }
}
