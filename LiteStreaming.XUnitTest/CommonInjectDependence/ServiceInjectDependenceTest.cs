using Microsoft.Extensions.DependencyInjection;
using Application.CommonInjectDependence;
using Application.Streaming;
using Application.Administrative;

namespace CommonInjectDependence;
public class ServiceInjectDependenceTest
{
    [Fact]
    public void AddServices_Should_Register_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services?.AddServicesWebApiApp();
        
        // Assert
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(UserService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(CustomerService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(PlaylistPersonalService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(MerchantService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(BandService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(MusicService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(PlaylistService)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(AlbumService)));
    }

    [Fact]
    public void AddServices_Should_Register_Administrative_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services?.AddServicesAdministrativeApp();

        // Assert
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(AdminAccountService)));
    }

}