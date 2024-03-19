using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account.Profile;
public class MusicProfile : AutoMapper.Profile
{
    public MusicProfile() 
    {
        CreateMap<MusicDto, Music>()
            .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.AlbumId ))
            .ReverseMap();

        CreateMap<Music, MusicDto>()
            .ForMember(dest => dest.AlbumId, opt => opt.MapFrom(src => src.Album.Id))
            .ReverseMap();

        CreateMap<AlbumDto, Album>()
            .ReverseMap();
    }
}
