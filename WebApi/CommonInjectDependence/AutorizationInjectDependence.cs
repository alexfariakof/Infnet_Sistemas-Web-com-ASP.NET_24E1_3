using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Authentication;

namespace WebApi.CommonInjectDependence;
public static class AutorizationInjectDependence
{
    public static IServiceCollection AddAuthConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        SigningConfigurations signingConfigurations = new SigningConfigurations();
        services.AddSingleton(signingConfigurations);

        TokenConfiguration tokenConfigurations = new TokenConfiguration();
        new ConfigureFromConfigurationOptions<TokenConfiguration>(
            configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

        services.AddSingleton(tokenConfigurations);
        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            Microsoft.IdentityModel.Tokens.TokenValidationParameters paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = tokenConfigurations.Audience;
            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

            // Validates the signing of a received token
            paramsValidation.ValidateIssuerSigningKey = true;

            // Checks if a received token is still valid
            paramsValidation.ValidateLifetime = true;

            // Tolerance time for the expiration of a token (used in case
            // of time synchronization problems between different
            // computers involved in the communication process)
            paramsValidation.ClockSkew = TimeSpan.Zero;
        });

        // Enables the use of the token as a means of
        // authorizing access to this project's resources
        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().Build());
        });
        return services;
    }
}
