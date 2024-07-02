using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency;
public class AdminAccountRepository : BaseRepository<AdminAccount>, IRepository<AdminAccount>
{
    public RegisterContextAdmin Context { get;  }
    public AdminAccountRepository(RegisterContextAdmin context) : base(context)
    {
        Context = context;
    }

    public override void Save(AdminAccount entity)
    {
        entity.PerfilType = Context.Set<Perfil>().Find(entity.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(AdminAccount entity)
    {
        entity.PerfilType = Context.Set<Perfil>().Find(entity.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Update(entity);
        Context.SaveChanges();
    }
}