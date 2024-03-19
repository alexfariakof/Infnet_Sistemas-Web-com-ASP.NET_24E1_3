using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Repository.Mapping.Account;

namespace Repository.Mapping;
public class SignitureMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 5;

        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_SignitureMapTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new SignitureMap();

            configuration.Configure(builder.Entity<Signature>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Signature));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();
            // Act
            var idProperty = entityType?.FindProperty("Id");
            var activeProperty = entityType?.FindProperty("Active");
            var dtActivationProperty = entityType?.FindProperty("DtActivation");
            var flatNavigation = entityType?.FindNavigation("Flat");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(activeProperty);
            Assert.NotNull(dtActivationProperty);

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(activeProperty.IsNullable);
            Assert.False(dtActivationProperty.IsNullable);
            Assert.NotNull(flatNavigation);
            Assert.False(flatNavigation.IsCollection);
            Assert.NotNull(flatNavigation.ForeignKey);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}