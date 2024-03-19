using Application.Account.Dto;

namespace Application.Account.Interfaces;
public interface IUserService
{    AuthenticationDto Authentication(LoginDto dto);
}
