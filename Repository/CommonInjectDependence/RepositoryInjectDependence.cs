﻿using Microsoft.Extensions.DependencyInjection;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Repository.Repositories;

namespace Repository.CommonInjectDependence;
public static class RepositoryInjectDependence
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<Customer>), typeof(CustomerRepository));
        services.AddScoped(typeof(IRepository<Merchant>), typeof(MerchantRepository));
        services.AddScoped(typeof(IRepository<Flat>), typeof(FlatRepository));
        services.AddScoped(typeof(IRepository<Band>), typeof(BandRepository));
        services.AddScoped(typeof(IRepository<Music>), typeof(MusicRepository));
        services.AddScoped(typeof(IRepository<Playlist>), typeof(PlaylistRepository));
        return services;
    }
}