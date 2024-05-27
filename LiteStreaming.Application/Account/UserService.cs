﻿using Application.Account.Dto;
using Application.Account.Interfaces;
using Application.Authentication;
using Application.Shared.Dto;
using AutoMapper;
using Domain.Account.Agreggates;
using LiteStreaming.Cryptography;
using Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Account;
public class UserService : IUserService
{
    private readonly ICrypto _crypto = Crypto.Instance;
    private readonly IRepository<User> _userRepository;
    private readonly SigningConfigurations _singingConfiguration;
    public UserService(IMapper mapper, IRepository<User> userRepository, SigningConfigurations singingConfiguration)
    {
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
            credentialsValid = user is not  null && !String.IsNullOrEmpty(user.Login.Password) && !String.IsNullOrEmpty(user.Login.Email) && (_crypto.IsEquals(dto?.Password ?? "", user.Login.Password));
        }

        if (credentialsValid)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim("UserType", user.PerfilType.Description),
                new Claim("UserId",  user.Id.ToString())
            });
            
            var token = _singingConfiguration.CreateAccessToken(identity);
            return new AuthenticationDto
            {
                AccessToken = token,
                Authenticated = true,                
                UserType = user.PerfilType.Description
            };
        }
        throw new ArgumentException("Usuário Inválido!");
    }
}