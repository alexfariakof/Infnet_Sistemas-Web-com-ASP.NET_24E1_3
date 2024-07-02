using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using LiteStreaming.Application.Abstractions;
using LiteStreaming.Application.Core.Interfaces.Query;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
public class GenreService : ServiceBase<GenreDto, Genre>, IService<GenreDto>, IGenreService, IFindAll<GenreDto>
{
    public GenreService(IMapper mapper, IRepository<Genre> GenreRepository) : base(mapper, GenreRepository)  { }
    public override GenreDto Create(GenreDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Gênero já existente.");

        Genre Genre = Mapper.Map<Genre>(dto);
        Repository.Save(Genre);
        var result = Mapper.Map<GenreDto>(Genre);
        return result;
    }
    public override GenreDto FindById(Guid id)
    {
        var Genre = Repository.GetById(id);
        var result = Mapper.Map<GenreDto>(Genre);
        return result;
    }
    public override List<GenreDto> FindAll()
    {
        var Genres = Repository.FindAll().ToList();
        var result = Mapper.Map<List<GenreDto>>(Genres);
        return result;
    }

    public override List<GenreDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<GenreDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override GenreDto Update(GenreDto dto)
    {
        var Genre = Mapper.Map<Genre>(dto);
        Repository.Update(Genre);
        return Mapper.Map<GenreDto>(Genre);
    }
    public override bool Delete(GenreDto dto)
    {
        var Genre = Mapper.Map<Genre>(dto);
        Repository.Delete(Genre);
        return true;
    }
}