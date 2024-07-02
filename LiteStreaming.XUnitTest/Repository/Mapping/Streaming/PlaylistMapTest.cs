using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;

namespace Repository.Mapping.Streaming;
public class PlaylistMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 3;
        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase_PlaylistMapTest").Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new PlaylistMap(new Repository.Constants.DefaultValueSqlConstants());

            configuration.Configure(builder.Entity<Playlist>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Playlist));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var nameProperty = entityType?.FindProperty("Name");
            var backdropProperty = entityType?.FindProperty("Backdrop");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(nameProperty);
            Assert.NotNull(backdropProperty);
            Assert.False(backdropProperty.IsNullable);
            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(nameProperty.IsNullable);
            Assert.Equal(50, nameProperty.GetMaxLength());
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}
