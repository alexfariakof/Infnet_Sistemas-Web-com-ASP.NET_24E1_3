using Domain.Streaming.Agreggates;

namespace Repository.Repositories;
public class MusicRepository : RepositoryBase<Music>, IRepository<Music>
{
    public RegisterContext Context { get; set; }
    public MusicRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}