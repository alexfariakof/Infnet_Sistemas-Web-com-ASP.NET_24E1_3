using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.CommonInjectDependence;
using Application.CommonInjectDependence;
using WebApi.CommonInjectDependence;
using DataSeeders;
using DataSeeders.Implementations;

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
    builder.Services.AddDbContext<RegisterContext>(c => c.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
    builder.Services.AddTransient<IDataSeeder, DataSeeder>();
}
else
{

    builder.Services.AddDbContext<RegisterContext>(c =>
    {
        c.UseLazyLoadingProxies()
        .UseMySQL(builder.Configuration.GetConnectionString("MySqlConnectionString"));
    });
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
    app.Urls.Add("http://0.0.0.0:7204");
    app.Urls.Add("https://0.0.0.0:5146");
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} {appVersion}"); });
}


if (app.Environment.IsStaging())    
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

app.MapFallbackToFile("/index.html");


app.Run();