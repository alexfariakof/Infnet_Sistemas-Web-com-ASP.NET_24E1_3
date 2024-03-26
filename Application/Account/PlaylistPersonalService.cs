﻿using Application.Account.Dto;
using Application.Account.Dto.Interfaces;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Repository.Interfaces;

namespace Application.Account;
public class PlaylistPersonalService : ServiceBase<PlaylistPersonalDto, PlaylistPersonal>, IService<PlaylistPersonalDto>, IPlaylistPersonalService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Music> _musicRepository;
    public PlaylistPersonalService(
        IMapper mapper, 
        IRepository<PlaylistPersonal> playlistPersonalRepository,
        IRepository<Customer> cutomerRepository,
        IRepository<Music> musicRepository) : base(mapper, playlistPersonalRepository)  {
        _customerRepository = cutomerRepository;
        _musicRepository = musicRepository;
    }
    public override PlaylistPersonalDto Create(PlaylistPersonalDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Playlist já existente.");

        PlaylistPersonal playlist = Mapper.Map<PlaylistPersonal>(dto);
        playlist.Customer = this._customerRepository.Find(c => c.User.Id == dto.CustomerId).First();        
        playlist.Musics = this._musicRepository.Find(m => dto.Musics.Select(em => em.Id).Contains(m.Id)).ToList();
        Repository.Save(playlist);
        var result = Mapper.Map<PlaylistPersonalDto>(playlist);
        return result;
    }
    public override PlaylistPersonalDto FindById(Guid PlaylistId)
    {
        var playlist = Repository.GetById(PlaylistId);
        var result = Mapper.Map<PlaylistPersonalDto>(playlist);
        return result;
    }

    public override List<PlaylistPersonalDto> FindAll(Guid userId)
    {
        var playlists = Repository.GetAll().ToList();
        var result = Mapper.Map<List<PlaylistPersonalDto>>(playlists);
        return result;
    }
    public override PlaylistPersonalDto Update(PlaylistPersonalDto dto)
    {
        var playlist = this.Repository.GetById(dto.Id.Value);
        playlist.Customer = this._customerRepository.Find(c => c.Id == playlist.CustomerId).First();
        playlist.Musics = this._musicRepository.Find(m => dto.Musics.Select(em => em.Id).Contains(m.Id)).ToList();
        Repository.Update(playlist);
        return Mapper.Map<PlaylistPersonalDto>(playlist);
    }
    public override bool Delete(PlaylistPersonalDto dto)
    {
        var Playlist = Mapper.Map<PlaylistPersonal>(dto);
        Repository.Delete(Playlist);
        return true;
    }
}