using Domain.Streaming.Agreggates;

namespace Repository.Repositories;
public class BandRepository : RepositoryBase<Band>, IRepository<Band>
{
    public RegisterContext Context { get; set; }
    public BandRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }   
}