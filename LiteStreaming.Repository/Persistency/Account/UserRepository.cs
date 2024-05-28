﻿using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Repository.Abastractions;
using Repository.Interfaces;

namespace Repository.Persistency.Account;
public class UserRepository : BaseRepository<User>, IRepository<User>
{
    private new RegisterContext Context { get; set; }
    public UserRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }

    public override void Save(User entity)
    {
        entity.PerfilType = this.Context.Set<PerfilUser>().Find(entity.PerfilType.Id);
        Context.Add(entity);
        Context.SaveChanges();
    }

    public override void Update(User entity)
    {
        entity.PerfilType = this.Context.Set<PerfilUser>().Find(entity.PerfilType.Id);
        Context.Update(entity);
        Context.SaveChanges();
    }
}