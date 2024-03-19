using Domain.Account.Agreggates;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Account.ValueObject;
public enum UserTypeEnum
{
    Invalid = 99,
    Customer = 2,
    Merchant = 3,
    Admin = 1
}
public record UserType
{
    [NotMapped]
    public UserTypeEnum Type { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public virtual IList<User> Users { get; set; }
    public UserType() { }
    public UserType(UserTypeEnum type)
    {
        Id = (int)type;
        Type = type;
        Description = SetDescription(type);
    }

    private string SetDescription(UserTypeEnum userType = UserTypeEnum.Invalid)
    {
        if (UserTypeEnum.Customer == userType)
        {
            return "Customer";
        }
        else if (UserTypeEnum.Merchant == userType)
        {
            return "Merchant";
        }
        else if (UserTypeEnum.Admin == userType)
        {
            return "Admin";
        }

        return userType.ToString();
    }
}