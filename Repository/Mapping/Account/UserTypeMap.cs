using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Account.ValueObject;

namespace Repository.Mapping.Transactions;
public class UserTypeMap : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.ToTable("UserType");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasConversion<int>();
        builder.Property(x => x.Description).IsRequired();


        builder.HasData
        (
            new UserType(UserTypeEnum.Admin) { Description = "Admin" },
            new UserType(UserTypeEnum.Customer) { Description = "Customer" } ,
            new UserType(UserTypeEnum.Merchant) { Description = "Merchant" }
        );
    }
}