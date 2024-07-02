using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using LiteStreaming.Application.Abstractions;
using LiteStreaming.Application.Core.Interfaces.Query;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
public class AlbumService : ServiceBase<AlbumDto, Album>, IService<AlbumDto>, IAlbumService, IFindAll<AlbumDto>
{
    public AlbumService(IMapper mapper, IRepository<Album> albumRepository) : base(mapper, albumRepository)  { }
    public override AlbumDto Create(AlbumDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Album já existente.");

        Album album = Mapper.Map<Album>(dto);

        Repository.Save(album);
        var result = Mapper.Map<AlbumDto>(album);
        return result;
    }

    public override AlbumDto FindById(Guid id)
    {
        var album = Repository.GetById(id);
        var result = Mapper.Map<AlbumDto>(album);
        return result;
    }

    public override List<AlbumDto> FindAll()
    {
        var albums = Repository.FindAll().ToList();
        var result = Mapper.Map<List<AlbumDto>>(albums);
        return result;
    }

    public override List<AlbumDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<AlbumDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override AlbumDto Update(AlbumDto dto)
    {
        var album = Mapper.Map<Album>(dto);
        Repository.Update(album);
        return Mapper.Map<AlbumDto>(album);
    }

    public override bool Delete(AlbumDto dto)
    {
        var album = Mapper.Map<Album>(dto);
        Repository.Delete(album);
        return true;
    }
}