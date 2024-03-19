using Domain.Streaming.Agreggates;

namespace Repository.Repositories;
public class PlaylistRepository : RepositoryBase<Playlist>, IRepository<Playlist>
{
    public RegisterContext Context { get; set; }
    public PlaylistRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }   
}