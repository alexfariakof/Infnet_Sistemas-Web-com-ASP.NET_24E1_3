using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Account;
public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User"); 
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.DtCreated).HasDefaultValue(DateTime.Now);
                
        builder.HasOne(x => x.UserType)
                  .WithMany(cb => cb.Users)
                  .IsRequired();

        builder.OwnsOne<Login>(u => u.Login, l =>
        {
            l.Property(p => p.Email).HasColumnName("Email").HasMaxLength(150).IsRequired();
            l.Property(p => p.Password).HasColumnName("Password").HasMaxLength(255).IsRequired();
            l.WithOwner();
        });
    }
}