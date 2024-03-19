using __mock__;

namespace Domain.Notifications;
public class NotificationTest
{
    [Fact]
    public void Should_Succeed_Create_Notification_With_Valid_Parameters()
    {
        // Arrange
        var destination = MockCustomer.Instance.GetFaker();
        var sender = MockCustomer.Instance.GetFaker();
        var title = "Test Title";
        var message = "Test Message";
        var notificationType = NotificationType.User;

        // Act
        var notification = Notification.Create(title, message, notificationType, destination, sender);

        // Assert
        Assert.NotNull(notification);
        Assert.Equal(title, notification.Title);
        Assert.Equal(message, notification.Message);
        Assert.Equal(notificationType, notification.NotificationType);
        Assert.Equal(destination, notification.Destination);
        Assert.Equal(sender, notification.Sender);
        Assert.True(notification.DtNotification > DateTime.MinValue);
    }

    [Fact]
    public void Should_Throw_Exception_Create_Notification_With_Null_Destination()
    {
        // Arrange
        var title = "Test Title";
        var message = "Test Message";
        var notificationType = NotificationType.User;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Notification.Create(title, message, notificationType, null));
    }

    [Fact]
    public void Should_Throw_Exception_Create_Notification_With_Null_Title()
    {
        // Arrange
        var destination = MockCustomer.Instance.GetFaker();
        var message = "Test Message";
        var notificationType = NotificationType.User;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Notification.Create(null, message, notificationType, destination));
    }

    [Fact]
    public void Should_Throw_Exception_Create_Notification_With_Null_Message()
    {
        // Arrange
        var destination = MockCustomer.Instance.GetFaker();
        var title = "Test Title";
        var notificationType = NotificationType.User;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Notification.Create(title, null, notificationType, destination));
    }

    [Fact]
    public void Should_Throw_Exception_Create_Notification_With_User_Type_And_Null_Sender()
    {
        // Arrange
        var destination = MockCustomer.Instance.GetFaker();
        var title = "Test Title";
        var message = "Test Message";
        var notificationType = NotificationType.User;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Notification.Create(title, message, notificationType, destination, null));
    }
}