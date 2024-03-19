using Domain.Account.ValueObject;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;

namespace Domain.Account.Agreggates;
public class Merchant : AbstractAccount<Merchant>
{
    public string? CNPJ { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual IList<Transaction>? Transactions { get; set; }
    public override void CreateAccount(Merchant merchant, Address address,  Flat flat, Card card)
    {
        Name = merchant.Name;
        User.UserType = new UserType(UserTypeEnum.Merchant);
        CNPJ = merchant.CNPJ;
        merchant.Customer.Flat = flat;
        Customer = merchant.Customer;
        AddAdress(address);
        AddFlat(merchant.Customer, flat, card);
        AddCard(card);
    }
}
