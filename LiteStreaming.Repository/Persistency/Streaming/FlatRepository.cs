using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Streaming;
public class FlatRepository : BaseRepository<Flat>, IRepository<Flat>
{
    public RegisterContext Context { get; }
    public FlatRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}