using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Account;
public class MerchantRepository : BaseRepository<Merchant>, IRepository<Merchant>
{
    public RegisterContext Context { get; }
    public MerchantRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }

    public override void Save(Merchant entity)
    {
        entity.User.PerfilType = Context.Set<PerfilUser>().Find(entity.User.PerfilType.Id) ??  throw new ArgumentNullException();
        Context.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(Merchant entity)
    {
        entity.User.PerfilType = Context.Set<PerfilUser>().Find(entity.User.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Update(entity);
        Context.SaveChanges();
    }
}