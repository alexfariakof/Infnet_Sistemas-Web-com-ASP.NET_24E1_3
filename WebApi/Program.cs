﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.CommonInjectDependence;
using Application.CommonInjectDependence;
using WebApi.CommonInjectDependence;
using DataSeeders;
using DataSeeders.Implementations;
using Migrations.MySqlServer.CommonInjectDependence;
using Migrations.MsSqlServer.CommonInjectDependence;

var builder = WebApplication.CreateBuilder(args);

// Appliction Parameteres
var appName = "Serviços de Streaming";
var appVersion = "v1";
var appDescription = $"API Serviços de Streaming.";

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc(appVersion,
    new OpenApiInfo
    {
        Title = appName,
        Version = appVersion,
        Description = appDescription,
        Contact = new OpenApiContact
        {
            Name = "Alex Ribeiro de Faria",
            Url = new Uri("https://github.com/alexfariakof/Home_Broker_Chart")
        }
    });
});

if (builder.Environment.IsStaging())
{
    builder.Services.AddDbContext<RegisterContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
    builder.Services.AddTransient<IDataSeeder, DataSeeder>();
}
else if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<RegisterContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
    builder.Services.AddDbContext<RegisterContextAdministravtive>();
    builder.Services.ConfigureMsSqlServerMigrationsContext(builder.Configuration);
    builder.Services.ConfigureMySqlServerMigrationsContext(builder.Configuration);  
    builder.Services.AddTransient<IDataSeeder, DataSeeder>();

}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<RegisterContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnectionString")));
}

// Autorization Configuratons
builder.Services.AddAuthConfigurations(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper();

//Repositories
builder.Services.AddRepositories();

//Services
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsStaging())
{    
    app.Urls.Add("http://0.0.0.0:5146");
    app.Urls.Add("https://0.0.0.0:7204");
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} {appVersion}"); });
}

if (app.Environment.IsStaging() || app.Environment.IsDevelopment())    
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dataSeeder = services.GetRequiredService<IDataSeeder>();
        dataSeeder.SeedData();
    }
}
else
{
    app.UseHttpsRedirection();
}
    
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsProduction() || app.Environment.IsStaging())
    app.MapFallbackToFile("/index.html");

app.Run();