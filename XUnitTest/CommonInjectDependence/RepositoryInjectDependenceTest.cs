using Microsoft.Extensions.DependencyInjection;
using Repository.CommonInjectDependence;
using Repository;
using Repository.Repositories;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;

namespace CommonInjectDependence;
public class RepositoryInjectDependenceTest
{
    [Fact]
    public void AddRepositories_Should_Register_Repositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRepositories();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Customer>) && descriptor.ImplementationType == typeof(CustomerRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Merchant>) && descriptor.ImplementationType == typeof(MerchantRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Flat>) && descriptor.ImplementationType == typeof(FlatRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Band>) && descriptor.ImplementationType == typeof(BandRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Music>) && descriptor.ImplementationType == typeof(MusicRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Playlist>) && descriptor.ImplementationType == typeof(PlaylistRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Album>) && descriptor.ImplementationType == typeof(AlbumRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<PlaylistPersonal>) && descriptor.ImplementationType == typeof(PlaylistPersonalRepository)));
    }
}