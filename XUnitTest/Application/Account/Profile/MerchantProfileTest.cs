using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Account.Agreggates;

namespace Application.Account;
public class MerchantProfileTest
{

    [Fact]
    public void Map_MerchantDto_To_Merchant_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<MerchantProfile>();
        }));

        var merchantDto = MockMerchant.Instance.GetDtoFromMerchant(MockMerchant.Instance.GetFaker());

        // Act
        var merchant = mapper.Map<Merchant>(merchantDto);

        // Assert
        Assert.NotNull(merchant);
        Assert.Equal(merchantDto.Id, merchant.Id);
        Assert.Equal(merchantDto.Name, merchant.Name);
        //Assert.Equal(merchantDto.Email, merchant.Customer.Login.Email);
        //Assert.Equal(merchantDto.CPF, merchant.Customer.CPF);
        Assert.Equal(merchantDto.CNPJ, merchant.CNPJ);
        //Assert.NotNull(merchant.Customer.Phone);
        //Assert.Equal(merchantDto.Phone, merchant.Customer.Phone?.Number);
        Assert.NotNull(merchant.Addresses);
        Assert.NotNull(merchant.Cards);
    }

    [Fact]
    public void Map_Merchant_To_MerchantDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<MerchantProfile>();
        }));

        var merchant = MockMerchant.Instance.GetFaker();
        merchant.AddAdress(MockAddress.Instance.GetFaker());        
        merchant.AddCard(MockCard.Instance.GetFaker());

        var customer = new Customer()
        {
            Name = merchant.Name,
            Addresses = merchant.Addresses,
            Cards = merchant.Cards,
            User = merchant.Customer.User,
            CPF = merchant.Customer.CPF,
            Phone = merchant.Customer.Phone,
            Signatures = merchant.Signatures,
        };
        merchant.AddFlat(customer, MockFlat.Instance.GetFaker(), merchant.Cards.FirstOrDefault());

        // Act
        var merchantDto = mapper.Map<MerchantDto>(merchant);

        // Assert
        Assert.NotNull(merchantDto);
        Assert.Equal(merchant.Id, merchantDto.Id);
        Assert.Equal(merchant.Name, merchantDto.Name);
        Assert.Equal(merchant.Customer.User.Login.Email, merchantDto.Email);
        //Assert.Equal(merchant.Customer.CPF, merchantDto.CPF);
        Assert.Equal(merchant.CNPJ, merchantDto.CNPJ);
        Assert.NotNull(merchantDto.Phone);
        Assert.Equal(merchant.Customer.Phone?.Number, merchantDto.Phone);
        Assert.NotNull(merchantDto.Address);
        Assert.Equal(merchant.Addresses.First().Zipcode, merchantDto.Address.Zipcode);
        Assert.Equal(merchant.Addresses.First().Street, merchantDto.Address.Street);
        Assert.Equal(merchant.Addresses.First().Number, merchantDto.Address.Number);
        Assert.Equal(merchant.Addresses.First().Neighborhood, merchantDto.Address.Neighborhood);
        Assert.Equal(merchant.Addresses.First().City, merchantDto.Address.City);
        Assert.Equal(merchant.Addresses.First().State, merchantDto.Address.State);
        Assert.Equal(merchant.Addresses.First().Complement, merchantDto.Address.Complement);
        Assert.Equal(merchant.Addresses.First().Country, merchantDto.Address.Country);
        Assert.Null(merchantDto.Card);
    }
}
