using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;

namespace Repository.Mapping.Account;
public abstract class BaseAccountMap<T> : IEntityTypeConfiguration<T> where T : AbstractAccount<T>
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        builder.HasOne(x => x.User)            
            .WithMany().OnDelete(DeleteBehavior.NoAction);

        ConfigureCustom(builder);
    }
    protected abstract void ConfigureCustom(EntityTypeBuilder<T> builder);
}