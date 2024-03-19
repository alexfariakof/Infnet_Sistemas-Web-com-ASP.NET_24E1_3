using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebApi.CommonInjectDependence;
using Application.Authentication;

namespace CommonInjectDependence;
public class AuthorizationInjectDependenceTest
{
    [Fact]
    public void AddAuthConfigurations_ShouldAddAuthenticationAndAuthorizationConfigurations()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton(new SigningConfigurations());
        
        TokenConfiguration _tokenConfigurations = new TokenConfiguration();
        new ConfigureFromConfigurationOptions<TokenConfiguration>(
            configuration.GetSection("TokenConfigurations")).Configure(_tokenConfigurations);
        services.AddSingleton(_tokenConfigurations);
        services.AddSingleton(new TokenConfiguration());        

        // Act
        services.AddAuthConfigurations(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        Assert.NotNull(serviceProvider.GetService<SigningConfigurations>());
        Assert.NotNull(serviceProvider.GetService<TokenConfiguration>());

        // Assert authentication configurations
        var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authenticationOptions.DefaultAuthenticateScheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, authenticationOptions.DefaultChallengeScheme);

        // Assert JWT bearer configurations
        var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().Get(JwtBearerDefaults.AuthenticationScheme);
        var signingConfigurations = serviceProvider.GetRequiredService<SigningConfigurations>();
        var tokenConfigurations = serviceProvider.GetRequiredService<TokenConfiguration>();

        Assert.NotNull(jwtBearerOptions);
        Assert.NotNull(jwtBearerOptions.TokenValidationParameters);
        Assert.Equal(signingConfigurations.Key, jwtBearerOptions.TokenValidationParameters.IssuerSigningKey);
        Assert.Equal(tokenConfigurations.Audience, jwtBearerOptions.TokenValidationParameters.ValidAudience);
        Assert.Equal(tokenConfigurations.Issuer, jwtBearerOptions.TokenValidationParameters.ValidIssuer);
        Assert.True(jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey);
        Assert.True(jwtBearerOptions.TokenValidationParameters.ValidateLifetime);
        Assert.Equal(TimeSpan.Zero, jwtBearerOptions.TokenValidationParameters.ClockSkew);
        
        // Assert authorization policy
        var authorizationPolicy = serviceProvider.GetRequiredService<IAuthorizationPolicyProvider>().GetPolicyAsync("Bearer")?.Result;
        Assert.NotNull(authorizationPolicy);
        Assert.Contains(JwtBearerDefaults.AuthenticationScheme, authorizationPolicy.AuthenticationSchemes);
    }
}