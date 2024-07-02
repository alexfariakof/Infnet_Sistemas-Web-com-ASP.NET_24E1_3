using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Repository.Persistency.Abstractions.Interfaces;
using Repository.Persistency.Abstractions;

namespace Repository.Persistency.Account;
public class CustomerRepository : BaseRepository<Customer>, IRepository<Customer>
{
    public RegisterContext Context { get;  }
    public CustomerRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }

    public override void Save(Customer entity)
    {
        entity.User.PerfilType = Context.Set<PerfilUser>().Find(entity.User.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(Customer entity)
    {
        entity.User.PerfilType = Context.Set<PerfilUser>().Find(entity.User.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Update(entity);
        Context.SaveChanges();
    }
}