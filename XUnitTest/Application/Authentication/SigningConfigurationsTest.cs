using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Authentication;
public class SigningConfigurationsTest
{
    [Fact]
    public void SigningConfigurations_Should_Initialize_Correctly()
    {
        // Arrange & Act
        var signingConfigurations = new SigningConfigurations();

        // Assert
        Assert.NotNull(signingConfigurations.Key);
        Assert.NotNull(signingConfigurations.SigningCredentials);
    }

    [Fact]
    public void Key_Should_Be_RSA_SecurityKey()
    {
        // Arrange
        var signingConfigurations = new SigningConfigurations();

        // Assert
        Assert.IsType<RsaSecurityKey>(signingConfigurations.Key);
    }

    [Fact]
    public void SigningCredentials_Should_Be_Correct_Algorithm()
    {
        // Arrange
        var signingConfigurations = new SigningConfigurations();

        // Assert
        Assert.NotNull(signingConfigurations.SigningCredentials.Algorithm);
        Assert.Equal(SecurityAlgorithms.RsaSha256Signature, signingConfigurations.SigningCredentials.Algorithm);
    }

    [Fact]
    public void CreateToken_Should_Generate_Valid_Token()
    {
        // Arrange
        var signingConfigurations = new SigningConfigurations();
        var handler = new JwtSecurityTokenHandler();
        var userId = Guid.NewGuid();
        var tokenConfiguration = new TokenConfiguration
        {
            Issuer = "testIssuer",
            Audience = "testAudience",
            Seconds = 3600 // 1 hour expiration for testing
        };
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testUser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.UniqueName, "testUser"),
            new Claim(JwtRegisteredClaimNames.UniqueName, "Customer")
        });

        // Act
        var token = signingConfigurations.CreateToken(claimsIdentity, handler, userId, tokenConfiguration);

        // Assert
        Assert.NotNull(token);
        Assert.True(handler.CanReadToken(token));

        var validatedToken = handler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = tokenConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenConfiguration.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingConfigurations.Key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out _);

        Assert.NotNull(validatedToken);
    }
}
