namespace Application.Account.Dto.Interfaces;
public interface IMusicService
{
    MusicDto Create(MusicDto obj);
    List<MusicDto> FindAll(Guid userId);
    MusicDto FindById(Guid id);
    MusicDto Update(MusicDto obj);
    bool Delete(MusicDto obj);
}