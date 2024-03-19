using Application.Account.Dto;
using Application.Account.Interfaces;
using Application.Authentication;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Core.Aggreggates;
using Domain.Core.Interfaces;
using Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Account;
public class UserService : IUserService
{
    private readonly ICrypto _crypto = Crypto.GetInstance;
    private readonly IRepository<User> _userRepository;
    private readonly SigningConfigurations _singingConfiguration;
    private readonly TokenConfiguration _tokenConfiguration;
    public UserService(IMapper mapper, IRepository<User> userRepository, SigningConfigurations singingConfiguration, TokenConfiguration tokenConfiguration)
    {
        _singingConfiguration = singingConfiguration;
        _tokenConfiguration = tokenConfiguration;
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
            credentialsValid = user != null && !String.IsNullOrEmpty(user.Login.Password) && !String.IsNullOrEmpty(user.Login.Email) && (_crypto.Decrypt(user.Login.Password).Equals(dto.Password));
        }

        if (credentialsValid)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Login.Email, "Login"),
                new[]
                {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Login.Email),
                        new Claim("UserType", user.UserType.Description),
                });

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = _singingConfiguration.CreateToken(identity, handler, user.Id, _tokenConfiguration);

            return new AuthenticationDto
            {
                AccessToken = token,
                Authenticated = true,                
                UserType = user.UserType.Description
            };
        }
        throw new ArgumentException("Usuário Inválido!");
    }

}