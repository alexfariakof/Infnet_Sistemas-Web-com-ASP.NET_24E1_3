using AdministrativeApp.CommonDependenceInject;
using Microsoft.EntityFrameworkCore;
using Migrations.MsSqlServer.CommonInjectDependence;
using Migrations.MySqlServer.CommonInjectDependence;
using Application.CommonInjectDependence;
using Repository.CommonInjectDependence;
using Repository;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using LiteStreaming.AdministrativeApp.CommonDependenceInject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRepositoriesAdministrativeApp();
builder.Services.AddServicesAdministrativeApp();
builder.Services.AddAutoMapperAdministrativeApp();
builder.Services.AddAuthenticationCookeis();


// Cryptography 
builder.Services.AddServicesCryptography(builder.Configuration);
builder.Services.AddAdministrativeDataSeeders();
builder.Services.AddWebApiDataSeeders();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<RegisterContextAdmin>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlAdministrativeConnectionString")));
    builder.Services.AddDbContext<RegisterContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnectionString")));
}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<RegisterContextAdmin>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlAdministrativeConnectionString")));
}
else if (builder.Environment.EnvironmentName.Equals("InMemory"))
{
    builder.Services.AddDbContext<RegisterContextAdmin>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_Administrative_InMemory"));
    builder.Services.AddDbContext<RegisterContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
}
else
{
    builder.Services.AddDbContext<RegisterContextAdmin>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_Administrative_InMemory"));
    builder.Services.MsSqlServerMigrationsAdministrativeContext(builder.Configuration);
    builder.Services.MySqlServerMigrationsAdministrativeContext(builder.Configuration);
}

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("pt-BR") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

if (!app.Environment.IsProduction())
{
    app.RunAdministrativeAppDataSeeders();
    app.RunWebApiDataSeeders();
}

app.Run();