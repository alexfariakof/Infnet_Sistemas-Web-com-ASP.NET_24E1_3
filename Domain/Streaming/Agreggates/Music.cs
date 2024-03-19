using Domain.Account.Agreggates;
using Domain.Core.Aggreggates;
using Domain.Streaming.ValueObject;

namespace Domain.Streaming.Agreggates;
public class Music : BaseModel
{
    public string? Name { get; set; }
    public Duration Duration { get; set; } = 0;    
    public string Url { get; set; }
    public virtual Album Album { get; set; }
    public virtual IList<Genre> Genres { get; set; } = new List<Genre>();
    public virtual IList<PlaylistPersonal> PersonalPlaylists { get; set; } = new List<PlaylistPersonal>();
    public virtual IList<Playlist> Playlists { get; set; } = new List<Playlist>();
    public virtual IList<Flat> Flats { get; set; } = new List<Flat>();
}