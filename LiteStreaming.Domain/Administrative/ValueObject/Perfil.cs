using Domain.Administrative.Agreggates;
using Domain.Core.ValueObject;

namespace Domain.Administrative.ValueObject;

public record Perfil : BasePerfil
{
    public virtual IList<AdminAccount>? Users { get; set; }

    public Perfil() : base() { }

    public Perfil(UserType type) : base(type) { }

    public static implicit operator Perfil(UserType userType) => new Perfil(userType);

    public static implicit operator Perfil(int perfilId) => new Perfil((UserType)perfilId);

    public static implicit operator UserType(Perfil perfil) => (UserType)perfil.Id;

    public static bool operator ==(Perfil perfil, UserType userType) => perfil?.Id == (int)userType;

    public static bool operator !=(Perfil perfil, UserType userType) => perfil?.Id != (int)userType;
}
