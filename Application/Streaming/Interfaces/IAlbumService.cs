namespace Application.Account.Dto.Interfaces;
public interface IAlbumService
{
    AlbumDto Create(AlbumDto obj);
    List<AlbumDto> FindAll(Guid userId);
    AlbumDto FindById(Guid id);
    AlbumDto Update(AlbumDto obj);
    bool Delete(AlbumDto obj);
}