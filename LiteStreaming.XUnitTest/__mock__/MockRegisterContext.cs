using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Repository.Mapping.Account;
using Repository.Mapping.Streaming;
using Repository.Mapping.Transactions;
using Repository.Mapping.Notifications;

namespace __mock__;
public class MockRegisterContext : DbContext
{
    public MockRegisterContext(DbContextOptions<MockRegisterContext> options) : base(options) {}

    public DbSet<Customer> Custumer{ get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);        
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new MerchantMap());
        modelBuilder.ApplyConfiguration(new AddressMap());
        modelBuilder.ApplyConfiguration(new PlaylistPersonalMap(new Repository.Constants.DefaultValueSqlConstants()));
        modelBuilder.ApplyConfiguration(new SignitureMap());
        modelBuilder.ApplyConfiguration(new AlbumMap(new Repository.Constants.DefaultValueSqlConstants()));
        modelBuilder.ApplyConfiguration(new BandMap());
        modelBuilder.ApplyConfiguration(new FlatMap());
        modelBuilder.ApplyConfiguration(new MusicMap(new Repository.Constants.DefaultValueSqlConstants()));
        modelBuilder.ApplyConfiguration(new PlaylistMap(new Repository.Constants.DefaultValueSqlConstants()));
        modelBuilder.ApplyConfiguration(new CreditCardMap());
        modelBuilder.ApplyConfiguration(new CreditCardBrandMap());
        modelBuilder.ApplyConfiguration(new TransactionMap());
        modelBuilder.ApplyConfiguration(new NotificationMap());
    }
}