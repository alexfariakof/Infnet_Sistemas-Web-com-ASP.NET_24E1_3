using Domain.Account.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Account;
public class PlaylistPersonalRepository : BaseRepository<PlaylistPersonal>, IRepository<PlaylistPersonal>
{
    public RegisterContext Context { get; }
    public PlaylistPersonalRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}