using Domain.Core.Aggreggates;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Administrative.Dto;
public class AdminAccountDto : BaseDto
{
    [JsonIgnore]
    public override Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório!")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório!")]
    [EmailAddress(ErrorMessage = "O campo email é inválido!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório!")]
    [PasswordPropertyText]
    public string? Password { get; set; }

    [Required]
    public PerfilDto PerfilType { get; set; }

}
