using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Transactions.Agreggates;
using Repository.Mapping.Transactions;

namespace Repository.Mapping;
public class TransactionMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 5;           

        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_TransactionMapTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new TransactionMap();

            configuration.Configure(builder.Entity<Transaction>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Transaction));

            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();


            // Act
            var idProperty = entityType?.FindProperty("Id");
            var dtTransactionProperty = entityType?.FindProperty("DtTransaction");
            var descriptionProperty = entityType?.FindProperty("Description");
            var customerNavigation = entityType?.FindNavigation("Customer");

            var monetaryValueProperty = entityType?.FindNavigation("Value")?.ForeignKey.Properties.First();


            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(dtTransactionProperty);
            Assert.NotNull(descriptionProperty);

            Assert.True(idProperty.IsPrimaryKey());                
            Assert.False(dtTransactionProperty.IsNullable);                
            Assert.False(descriptionProperty.IsNullable);
            Assert.Equal(50, descriptionProperty.GetMaxLength());
            Assert.NotNull(customerNavigation);
            Assert.False(customerNavigation.IsCollection);
            Assert.NotNull(customerNavigation.ForeignKey);
            Assert.True(customerNavigation.ForeignKey.IsRequired);
            Assert.Equal("Id", customerNavigation.ForeignKey.Properties.First().Name);
            Assert.False(monetaryValueProperty?.IsNullable);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}