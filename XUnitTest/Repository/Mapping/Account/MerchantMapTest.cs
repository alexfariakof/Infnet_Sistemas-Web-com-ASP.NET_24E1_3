using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Repository.Mapping.Account;

namespace Repository.Mapping;
public class MerchantMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 10;

        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_MerchantMapTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new MerchantMap();

            configuration.Configure(builder.Entity<Merchant>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Merchant));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var userNavigation = entityType?.FindNavigation("User");
            var nameProperty = entityType?.FindProperty("Name");
            var cnpjProperty = entityType?.FindProperty("CNPJ");
            var addressesNavigation = entityType?.FindNavigation("Addresses");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(userNavigation);
            Assert.False(userNavigation.IsCollection);
            Assert.NotNull(userNavigation?.ForeignKey.DeleteBehavior);
            Assert.NotNull(nameProperty);
            Assert.NotNull(cnpjProperty);
            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(nameProperty.IsNullable);
            Assert.Equal(100, nameProperty.GetMaxLength());
            Assert.False(cnpjProperty.IsNullable);
            Assert.Equal(18, cnpjProperty.GetMaxLength());
            Assert.NotNull(addressesNavigation);
            Assert.True(addressesNavigation.IsCollection);
            Assert.NotNull(addressesNavigation?.ForeignKey.DeleteBehavior);

            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}
