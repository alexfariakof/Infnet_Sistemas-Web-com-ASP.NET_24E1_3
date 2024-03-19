using Domain.Core.Aggreggates;
using Domain.Core.ValueObject;

namespace Domain.Streaming.Agreggates;
public class Flat : BaseModel
{
    public String? Name { get; set; }
    public String? Description { get; set; }
    public Monetary Value { get; set; } = 0;
    public virtual IList<Album> Albums { get; set; } = new List<Album>();
    public virtual IList<Music> Musics { get; set; } = new List<Music>();
    public virtual IList<Playlist> Playlists { get; set; } = new List<Playlist>();
}

