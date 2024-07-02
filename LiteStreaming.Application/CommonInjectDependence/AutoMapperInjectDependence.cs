using Microsoft.Extensions.DependencyInjection;
using Application.Streaming.Profile;
using Application.Transactions.Profile;
using Application.Administrative.Profile;

namespace Application.CommonInjectDependence;
public static class AutoMapperInjectDependence
{
    public static IServiceCollection AddAutoMapperAdministrativeApp(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AdminAccountProfile).Assembly);
        services.AddAutoMapper(typeof(FlatProfile).Assembly);
        services.AddAutoMapper(typeof(GenreProfile).Assembly);
        return services;
    }

    public static IServiceCollection AddAutoMapperWebApiApp(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile).Assembly);
        services.AddAutoMapper(typeof(MerchantProfile).Assembly);
        services.AddAutoMapper(typeof(AddressProfile).Assembly);
        services.AddAutoMapper(typeof(CardProfile).Assembly);
        services.AddAutoMapper(typeof(BandProfile).Assembly);
        services.AddAutoMapper(typeof(MusicProfile).Assembly);
        services.AddAutoMapper(typeof(AlbumProfile).Assembly);        
        services.AddAutoMapper(typeof(PlaylistProfile).Assembly);
        services.AddAutoMapper(typeof(PlaylistPersonalProfile).Assembly);
        return services;
    }
}