using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using LiteStreaming.Application.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
public class FlatService: ServiceBase<FlatDto, Flat>, IService<FlatDto>, IFlatService
{
    public FlatService(IMapper mapper, IRepository<Flat> flatRepository) : base(mapper, flatRepository)  { }
    public override FlatDto Create(FlatDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Plano já existente.");

        Flat flat = Mapper.Map<Flat>(dto);
        Repository.Save(flat);
        var result = Mapper.Map<FlatDto>(flat);
        return result;
    }
    public override FlatDto FindById(Guid flatId)
    {
        var flat = Repository.GetById(flatId);
        var result = Mapper.Map<FlatDto>(flat);
        return result;
    }
    public override List<FlatDto> FindAll()
    {
        var flats = Repository.FindAll().ToList();
        var result = Mapper.Map<List<FlatDto>>(flats);
        return result;
    }

    public override List<FlatDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<FlatDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override FlatDto Update(FlatDto dto)
    {
        var flat = Mapper.Map<Flat>(dto);
        Repository.Update(flat);
        return Mapper.Map<FlatDto>(flat);
    }
    public override bool Delete(FlatDto dto)
    {
        var flat = Mapper.Map<Flat>(dto);
        Repository.Delete(flat);
        return true;
    }
}