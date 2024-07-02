using Domain.Core.Aggreggates;
using System.ComponentModel.DataAnnotations;

namespace Application.Streaming.Dto;
public class PlaylistDto : BaseDto, IValidatableObject
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O campo Backdrop é obrigatório.")]
    [Url(ErrorMessage = "Está não é uma url válida.")]
    public string? Backdrop { get; set; }
    public IList<MusicDto> Musics { get; set; } = new List<MusicDto>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.Items.TryGetValue("HttpMethod", out var httpMethod) && httpMethod is string method)
        {
            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Name is required for PUT requests.", new[] { nameof(Name) });
            }

            if (method.Equals("PUT", StringComparison.OrdinalIgnoreCase) && Id == Guid.Empty)
            {
                yield return new ValidationResult("ID is required for PUT requests.", new[] { nameof(Id) });
            }

            if (method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) && Id == Guid.Empty)
            {
                yield return new ValidationResult("ID is required for delete requests.", new[] { nameof(Id) });
            }
        }
    }
}