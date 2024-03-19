using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;

namespace Repository.Mapping.Account;
public class MerchantMap : BaseAccountMap<Merchant>
{
    protected override void ConfigureCustom(EntityTypeBuilder<Merchant> builder)
    {
        builder.Property(x => x.CNPJ).IsRequired().HasMaxLength(18);

        builder.HasOne(x => x.Customer)
                       .WithOne()
                       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Addresses).WithOne();
        builder.HasMany(x => x.Cards).WithOne();
        builder.HasMany(x => x.Signatures).WithOne();
    }
}
