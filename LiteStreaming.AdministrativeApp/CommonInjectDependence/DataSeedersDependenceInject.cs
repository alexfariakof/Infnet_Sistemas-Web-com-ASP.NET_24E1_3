using DataSeeders;
using DataSeeders.Administrative;


namespace AdministrativeApp.CommonDependenceInject;
public static class DataSeedersDependenceInject
{
    public static void AddAdministrativeDataSeeders(this IServiceCollection services)
    {
        services.AddTransient<IDataSeederAdmin, DataSeederAdmin>();
    }

    public static void RunAdministrativeAppDataSeeders(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dataSeeder = services.GetRequiredService<IDataSeederAdmin>();
            dataSeeder.SeedData();
        }
    }

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