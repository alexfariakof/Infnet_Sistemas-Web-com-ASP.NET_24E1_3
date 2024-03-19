namespace Application.Account.Dto.Interfaces;
public interface IBandService
{
    BandDto Create(BandDto obj);
    List<BandDto> FindAll(Guid userId);
    BandDto FindById(Guid id);
    BandDto Update(BandDto obj);
    bool Delete(BandDto obj);
}