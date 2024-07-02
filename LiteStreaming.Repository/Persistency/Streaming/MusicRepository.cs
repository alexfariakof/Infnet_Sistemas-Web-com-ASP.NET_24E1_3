using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Streaming;
public class MusicRepository : BaseRepository<Music>, IRepository<Music>
{
    public RegisterContext Context { get; }
    public MusicRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}