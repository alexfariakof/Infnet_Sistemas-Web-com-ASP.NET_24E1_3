using Microsoft.Extensions.DependencyInjection;
using Application.CommonInjectDependence;
using Application.Account;

namespace CommonInjectDependence;
public class ServiceInjectDependenceTest
{
    [Fact]
    public void AddServices_Should_Register_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddServices();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(CustomerService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(MerchantService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(BandService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(MusicService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(PlaylistService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(AlbumService)));
    }
}