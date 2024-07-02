using Microsoft.EntityFrameworkCore;
using Domain.Administrative.Agreggates;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Repository.Mapping.Administrative;
public class AdminAccountMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase_AdminAccountMapTest").Options;

        using (var context = new DbContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new AdminAccountMap();

            configuration.Configure(builder.Entity<AdminAccount>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(AdminAccount));

            // Act
            var idProperty = entityType.FindProperty("Id");
            var nameProperty = entityType.FindProperty("Name");
            var dtCreatedProperty = entityType.FindProperty("DtCreated");
            var perfilTypeNavigation = entityType.FindNavigation(nameof(AdminAccount.PerfilType));
            var loginNavigation = entityType.FindNavigation(nameof(AdminAccount.Login));

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(nameProperty);
            Assert.NotNull(dtCreatedProperty);
            Assert.NotNull(perfilTypeNavigation);
            Assert.NotNull(loginNavigation);

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(nameProperty.IsNullable);
            Assert.Equal(100, nameProperty.GetMaxLength());
            Assert.False(dtCreatedProperty.IsNullable);
            Assert.False(perfilTypeNavigation.IsCollection);
            Assert.NotNull(perfilTypeNavigation?.ForeignKey.DeleteBehavior);
            Assert.NotNull(loginNavigation);
        }
    }
}
