using Domain.Administrative.ValueObject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Administrative;
public class PerfilRepository : BaseRepository<Perfil>, IRepository<Perfil>
{
    public RegisterContextAdmin Context { get;  }
    public PerfilRepository(RegisterContextAdmin context) : base(context)
    {
        Context = context;
    }
    public override Perfil GetById(int id)
    {
        return Context.Set<Perfil>().Find(id) ?? new();
    }
}