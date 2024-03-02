﻿using Domain.Account.Agreggates;
using Domain.Transactions.ValueObject;

namespace Repository.Repositories;
public class MerchantRepository : RepositoryBase<Merchant>, IRepository<Merchant>
{
    public RegisterContext Context { get; set; }
    public MerchantRepository(RegisterContext context) : base(context)
    {
        Context = context;
    }
    public override void Save(Merchant entity)
    {
        var dsCreditCardBrand = this.Context.Set<CreditCardBrand>();
        foreach (var card in entity.Cards) 
            card.CardBrand = dsCreditCardBrand.Where(c => card.CardBrand != null &&  c.Id == (int)card.CardBrand.CardBrand).FirstOrDefault();

        this.Context.Add(entity.Customer);
        this.Context.Add(entity);       
        this.Context.SaveChanges();
    }
}