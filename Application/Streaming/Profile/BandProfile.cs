using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account.Profile;
public class BandProfile : AutoMapper.Profile
{
    public BandProfile() 
    {
        CreateMap<BandDto, Band>()
            .ForMember(dest => dest.Albums, opt => opt.MapFrom(src => new List<AlbumDto>() { src.Album }))
            .ReverseMap();

        CreateMap<Band, BandDto>()
            .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.Albums.FirstOrDefault()))
            .AfterMap((s, d) =>
            {

            });

        CreateMap<MusicDto, Music>().ReverseMap();
        CreateMap<AlbumDto, Album>().ReverseMap();
    }
}
