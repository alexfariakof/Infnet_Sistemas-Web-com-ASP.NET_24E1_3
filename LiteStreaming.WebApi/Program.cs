using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Migrations.MySqlServer.CommonInjectDependence;
using Migrations.MsSqlServer.CommonInjectDependence;
using Application.CommonInjectDependence;
using Repository.CommonInjectDependence;
using WebApi.CommonInjectDependence;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Appliction Parameteres
var appName = "Lite Streaming API";
var appVersion = "v1";
var appDescription = $"API Lite Streaming Serviços de Streaming.";

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
            Name = "Alex Ribeiro de Faria - Projeto Web API .Net Core Lite Streaming ",
            Url = new Uri("https://github.com/alexfariakof/prj-sistemas-web-.net-infnet/tree/main/LiteStreaming.WebApi")
        }
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Adicione o token JWT para fazer as requisições na APIs",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddWebApiDataSeeders();

if (builder.Environment.IsStaging())
{
    builder.Services.AddDbContext<RegisterContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
}
else if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<RegisterContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnectionString")));
}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<RegisterContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnectionString")));
}
else if (builder.Environment.EnvironmentName.Equals("MySqlServer"))
{
    builder.Services.AddDbContext<RegisterContext>(options => options.UseLazyLoadingProxies().UseMySQL(builder.Configuration.GetConnectionString("MySqlConnectionString")));
}

else
{
    builder.Services.AddDbContext<RegisterContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("Register_Database_InMemory"));
    builder.Services.MsSqlServerMigrationsApplicationContext(builder.Configuration);
    builder.Services.MySqlServerMigrationsApplicationContext(builder.Configuration);
}

//Add SigningConfigurations Configuratons
builder.Services.AddSigningConfigurations(builder.Configuration);

// Add AutoAuthConfigurations Configuratons
builder.Services.AddAutoAuthenticationConfigurations();

// Autorization Configurations Identity Server STS
//builder.Services.AddIdentityServerConfigurations(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapperWebApiApp();

// Repositories
builder.Services.AddRepositoriesWebApiApp();

// Services
builder.Services.AddServicesWebApiApp();

// Cryptography 
builder.Services.AddServicesCryptography(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsStaging())
{
    app.Urls.Add("http://0.0.0.0:5146");
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.EnvironmentName.Equals("MySqlServer"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} {appVersion}"); });
    app.RunWebApiDataSeeders();
}
else
    app.UseHttpsRedirection();


if (app.Environment.IsStaging())
{
    app.UseAuthentication();
    app.UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("index.html");
    });
}
else
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCertificateForwarding();
    app.MapControllers();
}


app.Run();