using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Repository.Mapping.Account;

namespace Repository.Mapping;
public class PlaylistPersonalMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 4;
        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_PlaylistPersonalMapTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new PlaylistPersonalMap();

            configuration.Configure(builder.Entity<PlaylistPersonal>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(PlaylistPersonal));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();
            // Act
            var idProperty = entityType?.FindProperty("Id");
            var nameProperty = entityType?.FindProperty("Name");
            var isPublicProperty = entityType?.FindProperty("IsPublic");
            var dtCreatedProperty = entityType?.FindProperty("DtCreated");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(nameProperty);
            Assert.NotNull(isPublicProperty);
            Assert.NotNull(dtCreatedProperty);

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(nameProperty.IsNullable);
            Assert.Equal(50, nameProperty.GetMaxLength());
            Assert.False(isPublicProperty.IsNullable);
            Assert.False(dtCreatedProperty.IsNullable);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}
