using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Transactions.Agreggates;
using Repository.Mapping.Transactions;

namespace Repository.Mapping;
public class CardMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        // Existe Uma propiedade Fantama pois só estamos validando 8
        const int PROPERTY_COUNT = 9;  

        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_CardMap")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new CardMap();

            configuration.Configure(builder.Entity<Card>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Card));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var activeProperty = entityType?.FindProperty("Active");
            var numberProperty = entityType?.FindProperty("Number");                
            var cvvProperty = entityType?.FindProperty("CVV");
            var cardBrandNavigation = entityType?.FindNavigation("CardBrand");


            var validateProperty = entityType?.FindNavigation("Validate")?.ForeignKey.Properties.First();
            var limitProperty = entityType?.FindNavigation("Limit")?.ForeignKey.Properties.First();
            var transactionsNavigation = entityType?.FindNavigation("Transactions");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(activeProperty);
            Assert.NotNull(numberProperty);
            Assert.NotNull(validateProperty);
            Assert.NotNull(cvvProperty);
            Assert.NotNull(limitProperty);
            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(activeProperty.IsNullable);
            Assert.False(numberProperty.IsNullable);
            Assert.Equal(19, numberProperty.GetMaxLength());
            Assert.False(validateProperty.IsNullable);
            Assert.False(cvvProperty.IsNullable);
            Assert.Equal(255, cvvProperty.GetMaxLength());
            Assert.NotNull(cardBrandNavigation);
            Assert.NotNull(cardBrandNavigation);
            Assert.False(cardBrandNavigation.IsCollection);
            Assert.NotNull(cardBrandNavigation.ForeignKey);
            Assert.True(cardBrandNavigation.ForeignKey.IsRequired);
            Assert.False(limitProperty.IsNullable);
            Assert.NotNull(transactionsNavigation);
            Assert.True(transactionsNavigation.IsCollection);
            Assert.NotNull(transactionsNavigation?.ForeignKey.DeleteBehavior);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}