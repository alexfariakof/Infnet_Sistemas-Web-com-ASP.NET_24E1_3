using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;

namespace Repository.Mapping.Streaming;
public class MusicMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 8;
        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>().UseInMemoryDatabase(databaseName: "InMemoryDatabase_MusicMapTest").Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new MusicMap(new Repository.Constants.DefaultValueSqlConstants());

            configuration.Configure(builder.Entity<Music>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Music));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var nameProperty = entityType?.FindProperty("Name");
            var urlProperty = entityType?.FindProperty("Url");
            var durationProperty = entityType?.FindNavigation("Duration")?.ForeignKey.Properties.FirstOrDefault();
            var albumNavigation = entityType?.FindNavigation("Album");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(nameProperty);
            Assert.NotNull(durationProperty);
            Assert.NotNull(albumNavigation);
            Assert.NotNull(urlProperty);            

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(nameProperty.IsNullable);
            Assert.Equal(50, nameProperty.GetMaxLength());
            Assert.False(durationProperty.IsNullable);
            Assert.False(albumNavigation.IsCollection);
            Assert.NotNull(albumNavigation?.ForeignKey.DeleteBehavior);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}
