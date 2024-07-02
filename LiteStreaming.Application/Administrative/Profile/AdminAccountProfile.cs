using Application.Administrative.Dto;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;
using Domain.Core.ValueObject;

namespace Application.Administrative.Profile;
public class AdminAccountProfile : AutoMapper.Profile
{
    public AdminAccountProfile() 
    {
        CreateMap<AdminAccountDto, AdminAccount>()
            .ForMember(dest => dest.PerfilType, opt =>  opt.MapFrom(src => new Perfil((Perfil.UserType)src.PerfilType)))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => new Login() { Email = src.Email, Password = src.Password }))
            .ReverseMap();

        CreateMap<AdminAccount, AdminAccountDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Login.Email))
            .ForMember(dest => dest.PerfilType, opt => opt.MapFrom(src => (int)src.PerfilType.Id))
            .AfterMap((s, d) =>
            {
                d.Password = "********";
            });

        CreateMap<Perfil, PerfilDto>().ReverseMap();
        CreateMap<PerfilDto, Perfil>().ReverseMap();
    }
}
