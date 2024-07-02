using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Account.Agreggates;
using Domain.Notifications;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using Domain.Account.ValueObject;
using Repository.Mapping.Account;
using Repository.Mapping.Notifications;
using Repository.Mapping.Streaming;
using Repository.Mapping.Transactions;
using Repository.Constants;

namespace Repository.Abstractions;

public class BaseContext<TContext> : DbContext where TContext : DbContext
{
    public virtual DefaultValueSqlConstants? BASE_CONSTS { get; }
    public BaseContext(DbContextOptions<TContext> options) : base(options) { }

    // Definições das entidades
    public DbSet<User> User { get; set; }
    public DbSet<PerfilUser> PerfilUser { get; set; }
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
        // Account
        modelBuilder.ApplyConfiguration(new AddressMap());
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new MerchantMap());
        modelBuilder.ApplyConfiguration(new PerfilUserMap());
        modelBuilder.ApplyConfiguration(new PlaylistPersonalMap(BASE_CONSTS));
        modelBuilder.ApplyConfiguration(new SignitureMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        
        // Notifications
        modelBuilder.ApplyConfiguration(new NotificationMap());
        // Streaming
        modelBuilder.ApplyConfiguration(new AlbumMap(BASE_CONSTS));
        modelBuilder.ApplyConfiguration(new BandMap());
        modelBuilder.ApplyConfiguration(new FlatMap());
        modelBuilder.ApplyConfiguration(new GenreMap());
        modelBuilder.ApplyConfiguration(new MusicMap(BASE_CONSTS));
        modelBuilder.ApplyConfiguration(new PlaylistMap(BASE_CONSTS));
        // Transactions        
        modelBuilder.ApplyConfiguration(new CreditCardBrandMap());
        modelBuilder.ApplyConfiguration(new CreditCardMap());
        modelBuilder.ApplyConfiguration(new TransactionMap());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()));
        base.OnConfiguring(optionsBuilder);
    }
}
