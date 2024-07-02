using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Streaming;
public class AlbumRepository : BaseRepository<Album>, IRepository<Album>
{
    public RegisterContext Context { get; }
    public AlbumRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}