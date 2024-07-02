using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using LiteStreaming.Application.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
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

    public List<PlaylistPersonalDto> FindAll(Guid userId)
    {
        var customer = _customerRepository.Find(x => x.User.Id == userId).First();
        var playlists = Repository.Find(x => x.CustomerId == customer.Id).ToList();
        var result = Mapper.Map<List<PlaylistPersonalDto>>(playlists);
        return result;
    }

    public override List<PlaylistPersonalDto> FindAll()
    {
        var result = Mapper.Map<List<PlaylistPersonalDto>>(Repository.FindAll());
        return result;
    }

    public override List<PlaylistPersonalDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<PlaylistPersonalDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override PlaylistPersonalDto Update(PlaylistPersonalDto dto)
    {
        var playlist = this.Repository.GetById(dto.Id);
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