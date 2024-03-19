using Domain.Core.Aggreggates;
using Domain.Streaming.Agreggates;

namespace Domain.Account.Agreggates;
public class PlaylistPersonal : BaseModel
{
    public virtual Customer? Customer { get; set; }
    public bool IsPublic { get; set; }
    public DateTime DtCreated { get; set; }
    public string? Name { get; set; }
    public virtual IList<Music> Musics { get; set; } = new List<Music>();

}
