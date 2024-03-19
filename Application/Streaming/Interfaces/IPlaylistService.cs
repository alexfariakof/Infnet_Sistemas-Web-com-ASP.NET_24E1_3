using Application.Account.Dto;

namespace Application.Account.Dto.Interfaces;
public interface IPlaylistService
{
    PlaylistDto Create(PlaylistDto obj);
    List<PlaylistDto> FindAll(Guid userId);
    PlaylistDto FindById(Guid id);
    PlaylistDto Update(PlaylistDto obj);
    bool Delete(PlaylistDto obj);
}