using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;

namespace Repository.Persistency.Account;
public class UserRepository : BaseRepository<User>, IRepository<User>
{
    public RegisterContext Context { get; }
    public UserRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }

    public override void Save(User entity)
    {
        entity.PerfilType = Context.Set<PerfilUser>().Find(entity.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(User entity)
    {
        entity.PerfilType = Context.Set<PerfilUser>().Find(entity.PerfilType.Id) ?? throw new ArgumentNullException();
        Context.Update(entity);
        Context.SaveChanges();
    }
}