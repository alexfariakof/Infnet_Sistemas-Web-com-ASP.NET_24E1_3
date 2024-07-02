using Microsoft.Extensions.DependencyInjection;
using Application.Streaming;
using Application.Streaming.Dto;
using Application.Streaming.Interfaces;
using Application.Administrative;
using Application.Administrative.Interfaces;
using Microsoft.Extensions.Configuration;
using EasyCryptoSalt;
using LiteStreaming.Application.Abstractions;
using LiteStreaming.Application.Core.Interfaces.Query;
using Application.Administrative.Dto;
using Application.Streaming.Dto.Interfaces;

namespace Application.CommonInjectDependence;
public static class ServiceInjectDependence
{

    public static IServiceCollection AddServicesCryptography(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CryptoOptions>(configuration.GetSection("CryptoConfigurations"));
        services.AddSingleton<ICrypto, Crypto>();
        return services;
    }

    public static IServiceCollection AddServicesAdministrativeApp(this IServiceCollection services)
    {
        services.AddScoped<IService<AdminAccountDto>, AdminAccountService>();
        services.AddScoped<IAdminAuthService, AdminAccountService>();
        services.AddScoped<IService<FlatDto>, FlatService>();
        services.AddScoped<IService<GenreDto>, GenreService>();
        services.AddScoped<IFindAll<GenreDto>, GenreService>();
        services.AddScoped<IService<BandDto>, BandService>();
        services.AddScoped<IFindAll<BandDto>, BandService>();
        services.AddScoped<IService<AlbumDto>, AlbumService>();
        services.AddScoped<IFindAll<AlbumDto>, AlbumService>();
        services.AddScoped<IService<MusicDto>, MusicService>();
        services.AddScoped<IService<PlaylistDto>, PlaylistService>();
        return services;
    }

    public static IServiceCollection AddServicesWebApiApp(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IService<CustomerDto>, CustomerService>();
        services.AddScoped<IService<MerchantDto>, MerchantService>();
        services.AddScoped<IPlaylistPersonalService, PlaylistPersonalService>();        
        services.AddScoped<IService<BandDto>, BandService>();
        services.AddScoped<IService<MusicDto>, MusicService>();
        services.AddScoped<IService<PlaylistDto>, PlaylistService>();
        services.AddScoped<IService<AlbumDto>, AlbumService>();        
        return services;
    }
}

