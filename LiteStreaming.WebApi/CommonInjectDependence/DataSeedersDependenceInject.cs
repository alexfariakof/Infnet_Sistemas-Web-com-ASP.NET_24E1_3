using DataSeeders;

namespace WebApi.CommonInjectDependence;
public static class DataSeedersDependenceInject
{
    public static void AddWebApiDataSeeders(this IServiceCollection services)
    {
        services.AddTransient<IDataSeeder, DataSeeder>();
    }

    public static void RunWebApiDataSeeders(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dataSeeder = services.GetRequiredService<IDataSeeder>();
            dataSeeder.SeedData();
        }
    }
}