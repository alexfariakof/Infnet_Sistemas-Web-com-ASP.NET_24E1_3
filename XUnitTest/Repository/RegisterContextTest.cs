using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Notifications;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class RegisterContextTest
{
    [Fact]
    public void Should_Have_DbSets_RegisterContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Act
        using (var context = new RegisterContext(options))
        {
            // Assert
            Assert.NotNull(context.User);
            Assert.NotNull(context.UserType);
            Assert.NotNull(context.Customer);
            Assert.NotNull(context.Merchant);
            Assert.NotNull(context.Address);
            Assert.NotNull(context.PlaylistPersonal);
            Assert.NotNull(context.Signature);
            Assert.NotNull(context.Album);
            Assert.NotNull(context.Band);
            Assert.NotNull(context.Flat);
            Assert.NotNull(context.Music);
            Assert.NotNull(context.Playlist);
            Assert.NotNull(context.Card);
            Assert.NotNull(context.CardBrand);
            Assert.NotNull(context.Transaction);
            Assert.NotNull(context.Notification);
            Assert.NotNull(context.Genre);
        }
    }

    [Fact]
    public void Should_Apply_Configurations_RegisterContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemory_DataBase_RegisterContext")
            .Options;

        // Act
        using (var context = new RegisterContext(options))
        {
            // Assert
            var model = context.Model;
            Assert.True(model.FindEntityType(typeof(User)) != null);
            Assert.True(model.FindEntityType(typeof(UserType)) != null);
            Assert.True(model.FindEntityType(typeof(Customer)) != null);
            Assert.True(model.FindEntityType(typeof(Merchant)) != null);
            Assert.True(model.FindEntityType(typeof(Address)) != null);
            Assert.True(model.FindEntityType(typeof(PlaylistPersonal)) != null);
            Assert.True(model.FindEntityType(typeof(Signature)) != null);
            Assert.True(model.FindEntityType(typeof(Album)) != null);
            Assert.True(model.FindEntityType(typeof(Band)) != null);
            Assert.True(model.FindEntityType(typeof(Flat)) != null);
            Assert.True(model.FindEntityType(typeof(Music)) != null);
            Assert.True(model.FindEntityType(typeof(Playlist)) != null);
            Assert.True(model.FindEntityType(typeof(Card)) != null);
            Assert.True(model.FindEntityType(typeof(CreditCardBrand)) != null);
            Assert.True(model.FindEntityType(typeof(Transaction)) != null);
            Assert.True(model.FindEntityType(typeof(Notification)) != null);
            Assert.True(model.FindEntityType(typeof(Genre)) != null);

        }
    }
}