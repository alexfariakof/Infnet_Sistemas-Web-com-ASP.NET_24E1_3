using AutoMapper;
using Application.Transactions.Dto;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using Application.Transactions.Profile;

namespace Application.Transactions;
public class CardProfileTest
{
    [Fact]
    public void Map_CardDto_To_Card_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<CardProfile>();
        }));

        var cardDto = new CardDto
        {
            Number = "1234567812345678",
            Limit = 1000,
            Validate = DateTime.Now.AddYears(1),
            CVV = "123"
        };

        // Act
        var card = mapper.Map<Card>(cardDto);

        // Assert
        Assert.NotNull(card);
        Assert.Equal(cardDto.Number, card.Number);
        Assert.Equal(cardDto.Limit, card.Limit.Value);
        Assert.Equal(cardDto.Validate, card.Validate.Value);
        Assert.NotNull(card.CVV); 
    }

    [Fact]
    public void Map_Card_To_CardDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<CardProfile>();
        }));

        var expiryDate = new ExpiryDate(DateTime.Now.AddYears(1));

        var card = new Card
        {
            Number = "1234567812345678",
            Limit = 1000,
            Validate = expiryDate,
            CVV = "123"
        };

        // Act
        var cardDto = mapper.Map<CardDto>(card);

        // Assert
        Assert.NotNull(cardDto);
        Assert.Equal(card.Number, cardDto.Number);
        Assert.Equal(card.Limit.Value, cardDto.Limit);
        Assert.Equal(card.Validate.Value, cardDto.Validate);
        Assert.Equal("*************", cardDto.CVV);
    }
}
