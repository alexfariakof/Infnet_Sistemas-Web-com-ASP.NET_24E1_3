using Domain.Core.Aggreggates;

namespace Domain.Streaming.Agreggates;
public class Band : BaseModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Backdrop { get; set; }
    public virtual IList<Genre> Genres { get; set; } = new List<Genre>();
    public virtual List<Album> Albums { get; set; } = new List<Album>();
    public void AddAlbum(Album album) => this.Albums.Add(album);
}
