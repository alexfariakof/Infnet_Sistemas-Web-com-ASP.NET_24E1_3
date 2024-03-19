using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Repository.Mapping.Account;

namespace Repository.Mapping;
public class UserMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        const int PROPERTY_COUNT = 5;
        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_UserMapTestTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new UserMap();

            configuration.Configure(builder.Entity<User>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(User));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var loginNavigation = entityType?.FindNavigation(nameof(Login));
            var emailProperty = loginNavigation?.TargetEntityType.FindProperty(nameof(Login.Email));
            var passwordProperty = loginNavigation?.TargetEntityType.FindProperty(nameof(Login.Password));
            var dtCreatedProperty = entityType?.FindProperty("DtCreated");
            
            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(loginNavigation);
            Assert.NotNull(emailProperty);
            Assert.NotNull(passwordProperty);
            Assert.NotNull(dtCreatedProperty);

            Assert.True(idProperty?.IsPrimaryKey());
            Assert.False(loginNavigation.IsCollection);
            Assert.NotNull(loginNavigation?.ForeignKey.DeleteBehavior);
            Assert.False(emailProperty.IsNullable);
            Assert.Equal(150, emailProperty.GetMaxLength());
            Assert.False(passwordProperty.IsNullable);
            Assert.Equal(255, passwordProperty.GetMaxLength());
            Assert.False(dtCreatedProperty.IsNullable);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}