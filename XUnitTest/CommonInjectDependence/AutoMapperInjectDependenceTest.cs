using Microsoft.Extensions.DependencyInjection;
using Application.CommonInjectDependence;
using Application.Account.Profile;
using Application.Transactions.Profile;

namespace CommonInjectDependence;
public class AutoMapperInjectDependenceTest
{
    [Fact]
    public void AddAutoMapper_Should_Register_Profiles()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAutoMapper();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(CustomerProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(MerchantProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(AddressProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(CardProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(BandProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(MusicProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(AlbumProfile)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(PlaylistProfile)));
    }
}
