using Domain.Account.ValueObject;
using Domain.Core.Aggreggates;
using Domain.Core.ValueObject;
using Domain.Notifications;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;

namespace Domain.Account.Agreggates;
public abstract class AbstractAccount<T> : BaseModel
{
    public string? Name { get; set; }
    public virtual User User { get; set; } = new User();
    public virtual IList<Address> Addresses { get; set; } = new List<Address>();
    public virtual IList<Card> Cards { get; set; } = new List<Card>();    
    public virtual IList<Signature> Signatures { get; set; } = new List<Signature>();
    public virtual IList<Notification> Notifications { get; set; } = new List<Notification>();
    public abstract void CreateAccount(T obj, Address adress, Flat flat, Card card);    
    public void AddAdress(Address address) => this.Addresses.Add(address);
    public void AddCard(Card card) => this.Cards.Add(card);
    public void AddFlat(Customer customer, Flat flat, Card card)
    {
        IsValidCreditCard(card.Number ?? "");
        card.Id = Guid.NewGuid();
        card.Active = true;
        card.Limit = 1000;
        card.CardBrand = CreditCardBrand.IdentifyCard(card.Number ?? "");
        card.CreateTransaction(customer, new Monetary(flat.Value), flat.Description ?? "");
        DisableActiveSigniture();
        this.Signatures.Add(new Signature()
        {
            Active = true,
            Flat = flat,
            DtActivation = DateTime.Now,
        });
    }
    private void DisableActiveSigniture()
    {
        if (this.Signatures.Count > 0 && this.Signatures.Any(x => x.Active))
            this.Signatures.FirstOrDefault(x => x.Active).Active = false ;
    }

    private static void IsValidCreditCard(string creditCardNumber)
    {
        var cardInfo = CreditCardBrand.IdentifyCard(creditCardNumber);

        if (!cardInfo.IsValid)
            throw new ArgumentException($"Cartão { cardInfo.Name } inválido.");
        else if (cardInfo.CardBrand == CardBrand.Invalid)
            throw new ArgumentException($"Cartão { cardInfo.Name }.");                

    }
}
