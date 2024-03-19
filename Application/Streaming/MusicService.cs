using Application.Account.Dto;
using Application.Account.Dto.Interfaces;
using AutoMapper;
using Domain.Streaming.Agreggates;
using Repository;

namespace Application.Account;
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

    public override List<MusicDto> FindAll(Guid musicId)
    {
        var musics = Repository.GetAll().ToList();
        var result = Mapper.Map<List<MusicDto>>(musics);
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