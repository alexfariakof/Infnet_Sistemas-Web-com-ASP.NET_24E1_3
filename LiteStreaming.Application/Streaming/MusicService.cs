using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using LiteStreaming.Application.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
public class MusicService : ServiceBase<MusicDto, Music>, IService<MusicDto>, IMusicService
{
    public MusicService(IMapper mapper, IRepository<Music> musicRepository) : base(mapper, musicRepository)  { }
    public override MusicDto Create(MusicDto dto)
    {
        if (Repository.Exists(x => x.Name != null && x.Name == dto.Name))
            throw new ArgumentException("Musica já existente.");

        Music music = Mapper.Map<Music>(dto);

        Repository.Save(music);
        var result = Mapper.Map<MusicDto>(music);
        return result;
    }

    public override MusicDto FindById(Guid id)
    {
        var music = Repository.GetById(id);
        var result = Mapper.Map<MusicDto>(music);
        return result;
    }

    public override List<MusicDto> FindAll()
    {
        var musics = Repository.FindAll();
        var result = Mapper.Map<List<MusicDto>>(musics);
        return result;
    }

    public override List<MusicDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<MusicDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override MusicDto Update(MusicDto dto)
    {
        var music = Mapper.Map<Music>(dto);
        Repository.Update(music);
        return Mapper.Map<MusicDto>(music);
    }

    public override bool Delete(MusicDto dto)
    {
        var music = Mapper.Map<Music>(dto);
        Repository.Delete(music);
        return true;
    }
}