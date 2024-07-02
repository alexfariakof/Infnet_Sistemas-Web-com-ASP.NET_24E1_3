using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Streaming;
public class BandRepository : BaseRepository<Band>, IRepository<Band>
{
    public RegisterContext Context { get; }
    public BandRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}