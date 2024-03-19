using Application.Account.Dto;

namespace Application.Account.Dto.Interfaces;
public interface IPlaylistPersonalService
{
    PlaylistPersonalDto Create(PlaylistPersonalDto obj);
    List<PlaylistPersonalDto> FindAll(Guid userId);
    PlaylistPersonalDto FindById(Guid id);
    PlaylistPersonalDto Update(PlaylistPersonalDto obj);
    bool Delete(PlaylistPersonalDto obj);
}