﻿using Microsoft.Extensions.DependencyInjection;
using Repository.CommonInjectDependence;
using Repository.Persistency;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Repository.Interfaces;
using Domain.Administrative.Agreggates;
using Repository.Interfaces.Administrative;
using Repository.Persistency.Administrative;

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
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<User>) && descriptor.ImplementationType == typeof(UserRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Customer>) && descriptor.ImplementationType == typeof(CustomerRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Merchant>) && descriptor.ImplementationType == typeof(MerchantRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Flat>) && descriptor.ImplementationType == typeof(FlatRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Band>) && descriptor.ImplementationType == typeof(BandRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Music>) && descriptor.ImplementationType == typeof(MusicRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Playlist>) && descriptor.ImplementationType == typeof(PlaylistRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Album>) && descriptor.ImplementationType == typeof(AlbumRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<PlaylistPersonal>) && descriptor.ImplementationType == typeof(PlaylistPersonalRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<Genre>) && descriptor.ImplementationType == typeof(GenreRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(ICreditCardBrandRepository) && descriptor.ImplementationType == typeof(CreditCardBrandRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IUserTypeRepository) && descriptor.ImplementationType == typeof(UserTypeRepository)));
    }

    [Fact]
    public void AddRepositories_Should_Register_Administrative_Repositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRepositoriesAdministrativeApp();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IRepository<AdministrativeAccount>) && descriptor.ImplementationType == typeof(AdminAccountRepository)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(IPerfilRepository) && descriptor.ImplementationType == typeof(PerfilRepository)));
    }
}