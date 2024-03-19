using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Application.Authentication;
public class SigningConfigurations
{
    public SecurityKey Key { get; }
    public SigningCredentials SigningCredentials { get; }
    public SigningConfigurations()
    {
        using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048))
        {
            Key = new RsaSecurityKey(provider.ExportParameters(true));
        }

        SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
    }
    public string CreateToken(ClaimsIdentity identity, JwtSecurityTokenHandler handler, Guid UserId, TokenConfiguration tokenConfiguration)
    {
        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate + TimeSpan.FromSeconds(tokenConfiguration.Seconds);

        Microsoft.IdentityModel.Tokens.SecurityToken securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Issuer = tokenConfiguration.Issuer,
            Audience = tokenConfiguration.Audience,
            SigningCredentials = SigningCredentials,
            Subject = identity,
            NotBefore = createDate,
            Expires = expirationDate,
            Claims = new Dictionary<string, object> { { "UserId", UserId } },
        });

        string token = handler.WriteToken(securityToken);
        return token;
    }
}