using Application.Administrative.Dto;
using Application.Shared.Dto;

namespace Application.Administrative.Interfaces;
public interface IAdminAuthService
{
    AdminAccountDto Authentication(LoginDto dto);
}
