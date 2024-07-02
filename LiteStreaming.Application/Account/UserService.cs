using Application.Streaming.Dto;
using Application.Streaming.Interfaces;
using Application.Authentication;
using Application.Shared.Dto;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using EasyCryptoSalt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Streaming;
public class UserService : IUserService
{
    private readonly ICrypto _crypto;
    private readonly IRepository<User> _userRepository;
    private readonly SigningConfigurations _singingConfiguration;
    public UserService(IMapper mapper, IRepository<User> userRepository, SigningConfigurations singingConfiguration, ICrypto crypto)
    {
        _crypto = crypto;
        _singingConfiguration = singingConfiguration;
        _userRepository = userRepository;
    }

    public AuthenticationDto Authentication(LoginDto dto)
    {
        bool credentialsValid = false;

        var user = _userRepository.Find(u => u.Login.Email.Equals(dto.Email)).FirstOrDefault();
        if (user == null)
            throw new ArgumentException("Usuário inexistente!");
        else
        {
            credentialsValid = !String.IsNullOrEmpty(user.Login.Password) && !String.IsNullOrEmpty(user.Login.Email) && (_crypto.IsEquals(dto?.Password ?? "", user.Login.Password));
        }

        if (credentialsValid && user is not null)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim("role", ((PerfilUser.UserType)user.PerfilType.Id).ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            });

            var token = _singingConfiguration.CreateAccessToken(identity);
            return new AuthenticationDto
            {
                access_token = token,
                Authenticated = true,
                UserType = user.PerfilType.Description
            };
        }
        throw new ArgumentException("Usuário Inválido!");
    }
}