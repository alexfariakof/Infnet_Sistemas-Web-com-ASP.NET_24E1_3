using Domain.Streaming.Agreggates;

namespace Domain.Account;
public class MerchantTest
{
    [Fact]
    public void Should_Create_Account_With_Flat_Card_And_Playlist()
    {
        // Arrange
        var merchantMock = MockMerchant.Instance.GetFaker();
        var merchant = merchantMock;
        var flat = new Flat
        {
            Id = Guid.NewGuid(),
            Name = "Test Flat",
            Value = 50.0m,
            Description = "Test Description"
            
        };

        var card = MockCard.Instance.GetFaker();
        card.Active = true;

        // Act
        merchant.CreateAccount(merchantMock, MockAddress.Instance.GetFaker(), flat, card);

        // Assert
        Assert.Equal(merchantMock.Name, merchant.Name);
        Assert.Equal(merchantMock.User.Login, merchant.User.Login) ;
        Assert.Equal(merchantMock.CNPJ, merchant.CNPJ);
        Assert.Single(merchant.Cards, card);
    }
}