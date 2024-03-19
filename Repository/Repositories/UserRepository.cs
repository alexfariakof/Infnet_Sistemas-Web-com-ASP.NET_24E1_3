using Domain.Account.Agreggates;

namespace Repository.Repositories;
public class UserRepository : RepositoryBase<User>, IRepository<User>
{
    public RegisterContext Context { get; set; }
    public UserRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
}