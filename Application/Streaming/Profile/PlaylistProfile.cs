﻿using Application.Streaming.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account.Profile;
public class PlaylistProfile : AutoMapper.Profile
{
    public PlaylistProfile() 
    {
        CreateMap<PlaylistDto, Playlist>().ReverseMap();
        CreateMap<Playlist, PlaylistDto>();        
    }
}
