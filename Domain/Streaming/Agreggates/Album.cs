using Domain.Account.Agreggates;
using Domain.Core.Aggreggates;

namespace Domain.Streaming.Agreggates;
public class Album : BaseModel
{
    public string? Name { get; set; }
    public string? Backdrop { get; set; }    
    public virtual IList<Genre> Genres { get; set; } = new List<Genre>();
    public virtual IList<Flat> Flats { get; set; } = new List<Flat>();
    public virtual IList<Music> Musics { get; set; } = new List<Music>();
    public virtual IList<PlaylistPersonal> MusicPersonal { get; set; } = new List<PlaylistPersonal>();
    public void AddMusic(Music music) => this.Musics.Add(music);
    public void AddMusic(IList<Music> music) => music.ToList().ForEach(m => this.Musics.Add(m));
    public void AddMusic(PlaylistPersonal music) => this.MusicPersonal.Add(music);
    public void AddMusic(IList<PlaylistPersonal> music) => music.ToList().ForEach(m => this.MusicPersonal.Add(m));
}