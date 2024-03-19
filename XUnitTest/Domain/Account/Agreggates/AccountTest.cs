using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Moq;

namespace Domain.Account;
public class AbstractAccountTests
{
    [Fact]
    public void Should_AddCard_To_Card_List()
    {
        // Arrange
        var accountMock = new Mock<AbstractAccount<Customer>>();
        accountMock.SetupGet(a => a.Cards).Returns(new List<Card>());
        AbstractAccount<Customer> account = accountMock.Object;
        var card = MockCard.Instance.GetFaker();

        // Act
        account.AddCard(card);

        // Assert
        Assert.Contains(card, account.Cards);
    }

    [Fact]
    public void Should_Create_Transaction_And_AddSignature_When_Add_Flat()
    {
        // Arrange
        var accountMock = new Mock<AbstractAccount<Customer>>();
        accountMock.SetupGet(a => a.Cards).Returns(new List<Card>());
        accountMock.SetupGet(a => a.Signatures).Returns(new List<Signature>());
        var account = accountMock.Object;
        var flat = new Flat
        {
            Id = Guid.NewGuid(),
            Name = "Test Flat",
            Value = 50.0m,
            Description = "Test Description"
        };
        var card = MockCard.Instance.GetFaker();
        card.Active = true;
        accountMock.SetupGet(a => a.Cards).Returns(new List<Card> { card });

        // Act
        account.AddFlat(MockCustomer.Instance.GetFaker(), flat, card);

        // Assert
        Assert.Single(account.Signatures, s => s.Flat == flat && s.Active);
    }

    [Fact]
    public void Should_Disable_Active_Signature_DisableActiveSigniture()
    {
        // Arrange
        var accountMock = new Mock<AbstractAccount<Customer>>();
        accountMock.SetupGet(a => a.Signatures).Returns(new List<Signature>());
        var account = accountMock.Object;
        var activeSignature = new Signature { Active = true };
        var inactiveSignature = new Signature { Active = false };
        account.Signatures.Add(activeSignature);
        account.Signatures.Add(inactiveSignature);

        var flat = new Flat
        {
            Id = Guid.NewGuid(),
            Name = "Test Flat",
            Value = 50.0m,
            Description = "Test Description"
        };
        var cards = MockCard.Instance.GetListFaker(100);
        accountMock.SetupGet(a => a.Cards).Returns(cards);
        var card = cards.Where(c => c.Active == true).First();
        accountMock.SetupGet(a => a.Cards).Returns(new List<Card> { card });

        // Act
        account.AddFlat(MockCustomer.Instance.GetFaker(), flat, card);

        // Assert
        Assert.Single(account.Signatures, s => s.Flat == flat && s.Active);
        Assert.False(activeSignature.Active);
        Assert.False(inactiveSignature.Active);
    }

    [Fact]
    public void Should_Throw_Exception_When_Invalid_Credit_Card_AddFlat()
    {
        // Arrange
        var accountMock = new Mock<AbstractAccount<Customer>>();
        var account = accountMock.Object;
        account.Cards = new List<Card>();
        var flat = new Flat
        {
            Id = Guid.NewGuid(),
            Name = "Test Flat",
            Value = 50.0m,
            Description = "Test Description"
        };

        var cardMock = new Mock<Card>();
        var card = cardMock.Object;
        var mockCard = MockCard.Instance.GetFaker();
        card.Id = mockCard.Id;
        card.Number = "InvalidCardNumber"; // This will cause an invalid card
        card.Validate = mockCard.Validate;
        card.CVV = mockCard.CVV;
        card.Active = true;
        card.Limit = mockCard.Limit;

        // Act and Assert
        Assert.Throws<ArgumentException>(() => account.AddFlat(MockCustomer.Instance.GetFaker(), flat, card));
    }
}
