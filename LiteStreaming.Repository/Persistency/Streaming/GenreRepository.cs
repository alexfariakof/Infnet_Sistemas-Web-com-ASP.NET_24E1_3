using Domain.Streaming.Agreggates;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Streaming;
public class GenreRepository : BaseRepository<Genre>, IRepository<Genre>
{
    public RegisterContext Context { get; }
    public GenreRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}