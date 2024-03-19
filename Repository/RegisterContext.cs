using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Account.Agreggates;
using Domain.Notifications;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using Domain.Account.ValueObject;

namespace Repository;
public class RegisterContext: DbContext
{
    public RegisterContext(DbContextOptions<RegisterContext> options) : base(options) { }
    public DbSet<User> User { get; set; }    
    public DbSet<UserType> UserType { get; set; }
    public DbSet<Customer> Customer { get; set; }        
    public DbSet<Merchant> Merchant { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<PlaylistPersonal> PlaylistPersonal { get; set; }
    public DbSet<Signature> Signature { get; set; }
    public DbSet<Album> Album { get; set; }
    public DbSet<Band> Band { get; set; }
    public DbSet<Flat> Flat { get; set; }
    public DbSet<Music> Music { get; set; }    
    public DbSet<Playlist> Playlist { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Card> Card { get; set; }
    public DbSet<CreditCardBrand> CardBrand { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Notification> Notification { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegisterContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()));
        base.OnConfiguring(optionsBuilder);
    }
}