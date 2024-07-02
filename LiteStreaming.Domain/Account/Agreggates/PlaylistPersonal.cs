using Domain.Core.Aggreggates;
using Domain.Streaming.Agreggates;

namespace Domain.Account.Agreggates;
public class PlaylistPersonal : Base
{
    public string? Name { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual Guid CustomerId { get; set; }
    public bool IsPublic { get; set; }
    public DateTime DtCreated { get; set; } = DateTime.Now;    
    public virtual IList<Music> Musics { get; set; } = new List<Music>();
}