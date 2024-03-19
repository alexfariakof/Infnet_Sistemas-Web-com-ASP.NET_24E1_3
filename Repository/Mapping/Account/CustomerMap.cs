using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Account;
public class CustomerMap : BaseAccountMap<Customer>
{
    protected override void ConfigureCustom(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Birth).IsRequired();
        builder.Property(x => x.CPF).IsRequired().HasMaxLength(14);
        builder.OwnsOne<Phone>(e => e.Phone, c =>
        {
            c.Property(x => x.Number).HasColumnName("Phone").HasMaxLength(50).IsRequired();
            c.WithOwner();
        });
       
        builder.HasOne(x => x.Flat).WithMany();
        builder.HasMany(x => x.Addresses).WithOne();
        builder.HasMany(x => x.Cards).WithOne();
        builder.HasMany(x => x.Signatures).WithOne();
        builder.HasMany(x => x.Playlists).WithOne(x => x.Customer);

    }
}
