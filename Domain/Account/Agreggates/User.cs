using Domain.Account.ValueObject;
using Domain.Core.Aggreggates;

namespace Domain.Account.Agreggates;
public class User : BaseModel
{
    public Login Login { get; set; }
    public DateTime DtCreated { get; set; }
    public virtual UserType UserType { get; set; }    
}