using Domain.Account.ValueObject;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;

namespace Domain.Account.Agreggates;
public class Customer : AbstractAccount<Customer>
{
    private const string PLAYLIST_NAME = "Favoritas";
    public DateTime Birth { get; set; }
    public string? CPF { get; set; } = String.Empty;
    public Phone? Phone { get; set; }
    public virtual Flat Flat { get; set; }
    public virtual IList<PlaylistPersonal> Playlists { get; set; } = new List<PlaylistPersonal>();
    public virtual IList<Transaction> Transactions { get; set; } = new List<Transaction>();
    public override void CreateAccount(Customer customer, Address address, Flat flat, Card card)
    {
        Id = Guid.NewGuid();
        Name = customer.Name;
        User.UserType = new UserType(UserTypeEnum.Customer);
        Birth = customer.Birth;
        CPF = customer.CPF;
        Phone = customer.Phone;        
        Flat = flat;
        AddAdress(address);
        AddFlat(customer, flat, card);
        AddCard(card);
        CreatePlaylist(name: PLAYLIST_NAME, @public: false);
    }
    public void CreatePlaylist(string name, bool @public = true)
    {
        this.Playlists.Add(new PlaylistPersonal()
        {
            Name = name,
            IsPublic = @public,
            DtCreated = DateTime.Now,
            Customer = this
        });
    }        
}
