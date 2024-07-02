using Domain.Administrative.Agreggates;
using Domain.Core.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Administrative;
public class AdminAccountMap : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("Account");
        builder.HasKey(account => account.Id);
        builder.Property(account => account.Id).ValueGeneratedOnAdd();
        builder.Property(account => account.Name).IsRequired().HasMaxLength(100);        
        builder.Property(account => account.DtCreated).IsRequired();
        builder.HasOne(account => account.PerfilType).WithMany(perfil => perfil.Users).IsRequired();
        builder.OwnsOne<Login>(account => account.Login, dictonary =>
        {
            dictonary.HasIndex(login => login.Email).IsUnique();
            dictonary.Property(login => login.Email).HasColumnName("Email").HasMaxLength(150).IsRequired() ;            
            dictonary.Property(login => login.Password).HasColumnName("Password").HasMaxLength(255).IsRequired();
            dictonary.WithOwner();
        });
    }
}
