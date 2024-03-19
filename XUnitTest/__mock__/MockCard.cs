using Domain.Core.ValueObject;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using static Domain.Transactions.ValueObject.CreditCardBrand;
using Bogus;

namespace __mock__;
public class MockCard
{
    private static readonly Lazy<MockCard> _instance = new Lazy<MockCard>(() => new MockCard());

    public static MockCard Instance => _instance.Value;

    private readonly Faker<Card> _faker;

    private MockCard()
    {
        _faker = new Faker<Card>();
    }

    public Card GetFaker()
    {
        var validCreditCard = GenerateValidCreditCardNumber();
        var fakeCard = new Faker<Card>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Active, f => f.Random.Bool())
            .RuleFor(a => a.Limit, f => new Monetary(f.Random.Decimal(1000, 10000)))
            .RuleFor(a => a.Number, f => validCreditCard)
            .RuleFor(a => a.Validate, f => new ExpiryDate(new DateTime(DateTime.Now.Year + 5, f.Random.Int(1, 12), 1)))
            .RuleFor(a => a.CVV, f => f.Random.Int(100, 999).ToString())
            .RuleFor(a => a.CardBrand, f => CreditCardBrand.IdentifyCard(validCreditCard))
            .Generate();

        return fakeCard;
    }

    public List<Card> GetListFaker(int count)
    {
        var cardList = new List<Card>();
        for (var i = 0; i < count; i++)
        {
            cardList.Add(GetFaker());
        }
        return cardList;
    }

    private string GenerateValidCreditCardNumber()
    {
        var cardNumber = GenerateRandomCreditCardNumber();
        var brandInfo = IdentifyCard(cardNumber);

        while (brandInfo.CardBrand == CardBrand.Invalid || !brandInfo.IsValid)
        {
            cardNumber = GenerateRandomCreditCardNumber();
            brandInfo = IdentifyCard(cardNumber);
        }

        return cardNumber;
    }

    private string GenerateRandomCreditCardNumber()
    {
        var random = new Random();
        var number = string.Empty;

        for (int i = 0; i < 16; i++)
        {
            number += random.Next(0, 10).ToString();
        }

        return number;
    }

    private string GenerateRandomCreditCardNumber(CardBrand brand)
    {
        var random = new Random();
        var number = string.Empty;

        switch (brand)
        {
            case CardBrand.Visa:
                number += "4";
                break;
            case CardBrand.Mastercard:
                number += "5";
                break;
            case CardBrand.Amex:
                number += "3";
                break;
            case CardBrand.Discover:
                number += "6";
                break;
            case CardBrand.DinersClub:
                number += "3";
                break;
            case CardBrand.JCB:
                number += "35";
                break;
            default:
                break;
        }

        for (int i = number.Length; i < 16; i++)
        {
            number += random.Next(0, 10).ToString();
        }

        return number;
    }
}
