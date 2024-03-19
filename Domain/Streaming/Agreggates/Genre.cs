using Domain.Core.Aggreggates;

namespace Domain.Streaming.Agreggates;
public class Genre : BaseModel
{
    public string? Name { get; set; }
    public virtual IList<Album> Albums { get; set; } = new List<Album>();
    public virtual IList<Band> Bands { get; set; } = new List<Band>();
    public virtual IList<Music> Musics { get; set; } = new List<Music>();
    public virtual IList<Playlist> Playlists { get; set; } = new List<Playlist>();    
}