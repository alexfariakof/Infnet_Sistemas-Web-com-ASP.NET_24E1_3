using Domain.Administrative.ValueObject;
using Domain.Core.Aggreggates;

namespace Domain.Administrative.Agreggates;
public class AdminAccount : BaseAccount
{
    public string? Name { get; set; }
    public virtual Perfil PerfilType { get; set; }
}