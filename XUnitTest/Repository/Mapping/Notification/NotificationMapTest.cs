using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Domain.Notifications;
using Repository.Mapping.Notifications;

namespace Repository.Mapping;
public class NotificationMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        // Existem mais 2 Propiedades que não estam sendo Mapeadas e validadas
        const int PROPERTY_COUNT = 9;
        // Arrange
        var options = new DbContextOptionsBuilder<MockRegisterContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase_NotificationMapTest")
            .Options;

        using (var context = new MockRegisterContext(options))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new NotificationMap();

            configuration.Configure(builder.Entity<Notification>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Notification));
            var propsCount = entityType?.GetNavigations().Count() + entityType?.GetProperties().Count();

            // Act
            var idProperty = entityType?.FindProperty("Id");
            var titleroperty = entityType?.FindProperty("Title");
            var messageProperty = entityType?.FindProperty("Message");
            var dtNotificationProperty = entityType?.FindProperty("DtNotification");
            var notificationTypeProperty = entityType?.FindProperty("NotificationType");
            var destinationNavigation = entityType?.FindNavigation("Destination");
            var senderNavigation = entityType?.FindNavigation("Sender");
            
            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(titleroperty);
            Assert.NotNull(messageProperty);
            Assert.NotNull(dtNotificationProperty);
            Assert.NotNull(notificationTypeProperty);

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(titleroperty.IsNullable);
            Assert.Equal(150, titleroperty.GetMaxLength());
            Assert.False(messageProperty.IsNullable);
            Assert.Equal(250, messageProperty.GetMaxLength());
            Assert.False(dtNotificationProperty.IsNullable);
            Assert.False(notificationTypeProperty.IsNullable);
            Assert.NotNull(destinationNavigation);
            Assert.False(destinationNavigation.IsCollection);
            Assert.NotNull(destinationNavigation.ForeignKey);
            Assert.True(destinationNavigation.ForeignKey.IsRequired);
            Assert.Equal(DeleteBehavior.Cascade, destinationNavigation.ForeignKey.DeleteBehavior);
            Assert.NotNull(senderNavigation);
            Assert.False(senderNavigation.IsCollection);
            Assert.NotNull(senderNavigation.ForeignKey);
            Assert.False(senderNavigation.ForeignKey.IsRequired);
            Assert.Equal(PROPERTY_COUNT, propsCount);
        }
    }
}