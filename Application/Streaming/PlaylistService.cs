using Application.Account.Dto;
using Application.Account.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
public class PlaylistService : ServiceBase<PlaylistDto, Playlist>, IService<PlaylistDto>, IPlaylistService
{
    public PlaylistService(IMapper mapper, IRepository<Playlist> PlaylistRepository) : base(mapper, PlaylistRepository)  { }
    public override PlaylistDto Create(PlaylistDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Playlist já existente.");

        Playlist Playlist = Mapper.Map<Playlist>(dto);

        Repository.Save(Playlist);
        var result = Mapper.Map<PlaylistDto>(Playlist);
        return result;
    }
    public override PlaylistDto FindById(Guid PlaylistId)
    {
        var Playlist = Repository.GetById(PlaylistId);
        var result = Mapper.Map<PlaylistDto>(Playlist);
        return result;
    }

    public override List<PlaylistDto> FindAll(Guid userId)
    {
        var Playlists = Repository.GetAll().ToList();
        var result = Mapper.Map<List<PlaylistDto>>(Playlists);
        return result;
    }
    public override PlaylistDto Update(PlaylistDto dto)
    {
        var Playlist = Mapper.Map<Playlist>(dto);
        Repository.Update(Playlist);
        return Mapper.Map<PlaylistDto>(Playlist);
    }
    public override bool Delete(PlaylistDto dto)
    {
        var Playlist = Mapper.Map<Playlist>(dto);
        Repository.Delete(Playlist);
        return true;
    }
}