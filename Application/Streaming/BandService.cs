using Application.Account.Dto;
using Application.Account.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
public class BandService : ServiceBase<BandDto, Band>, IService<BandDto>, IBandService
{
    public BandService(IMapper mapper, IRepository<Band> bandRepository) : base(mapper, bandRepository)  { }
    public override BandDto Create(BandDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Banda já existente.");

        Band band = Mapper.Map<Band>(dto);

        Repository.Save(band);
        var result = Mapper.Map<BandDto>(band);
        return result;
    }
    public override BandDto FindById(Guid bandId)
    {
        var band = Repository.GetById(bandId);
        var result = Mapper.Map<BandDto>(band);
        return result;
    }

    public override List<BandDto> FindAll(Guid userId)
    {
        var bands = Repository.GetAll().ToList();
        var result = Mapper.Map<List<BandDto>>(bands);
        return result;
    }
    public override BandDto Update(BandDto dto)
    {
        var band = Mapper.Map<Band>(dto);
        Repository.Update(band);
        return Mapper.Map<BandDto>(band);
    }
    public override bool Delete(BandDto dto)
    {
        var band = Mapper.Map<Band>(dto);
        Repository.Delete(band);
        return true;
    }
}