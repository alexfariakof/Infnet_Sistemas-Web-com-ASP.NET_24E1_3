﻿using Application.Authentication.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Application.Authentication;
public class SigningConfigurations: ISigningConfigurations
{
    public readonly SecurityKey Key;
    public readonly SigningCredentials SigningCredentials;
    public readonly TokenConfiguration TokenConfiguration;
    public SigningConfigurations(IOptions<TokenOptions> options)
    {
        TokenConfiguration = new TokenConfiguration(options);
        using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048))
        {
            Key = new RsaSecurityKey(provider.ExportParameters(true));
        }

        SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
    }

    public string CreateAccessToken(ClaimsIdentity identity)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = TokenConfiguration.Issuer,
            Audience = this.TokenConfiguration.Audience,
            SigningCredentials = this.SigningCredentials,
            Subject = identity,
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(TokenConfiguration.Seconds),
        });

        string token = handler.WriteToken(securityToken);
        return token;
    }

    public string GenerateRefreshToken()
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor()
        {
            Audience = this.TokenConfiguration.Audience,
            Issuer = this.TokenConfiguration.Issuer,
            Claims = new Dictionary<string, object> { { "KEY", Guid.NewGuid() } },
            Expires = DateTime.UtcNow.AddDays(this.TokenConfiguration.DaysToExpiry)
        });
        return handler.WriteToken(securityToken);
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(refreshToken.Replace("Bearer ", "")) as JwtSecurityToken;
        return jwtToken?.ValidTo >= DateTime.UtcNow;
    }
}