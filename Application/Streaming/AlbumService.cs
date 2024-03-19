using Application.Account.Dto;
using Application.Account.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
public class AlbumService : ServiceBase<AlbumDto, Album>, IService<AlbumDto>, IAlbumService
{
    public AlbumService(IMapper mapper, IRepository<Album> AlbumRepository) : base(mapper, AlbumRepository)  { }
    public override AlbumDto Create(AlbumDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Album já existente.");

        Album Album = Mapper.Map<Album>(dto);

        Repository.Save(Album);
        var result = Mapper.Map<AlbumDto>(Album);
        return result;
    }
    public override AlbumDto FindById(Guid AlbumId)
    {
        var Album = Repository.GetById(AlbumId);
        var result = Mapper.Map<AlbumDto>(Album);
        return result;
    }

    public override List<AlbumDto> FindAll(Guid userId)
    {
        var Albums = Repository.GetAll().ToList();
        var result = Mapper.Map<List<AlbumDto>>(Albums);
        return result;
    }
    public override AlbumDto Update(AlbumDto dto)
    {
        var Album = Mapper.Map<Album>(dto);
        Repository.Update(Album);
        return Mapper.Map<AlbumDto>(Album);
    }
    public override bool Delete(AlbumDto dto)
    {
        var Album = Mapper.Map<Album>(dto);
        Repository.Delete(Album);
        return true;
    }
}