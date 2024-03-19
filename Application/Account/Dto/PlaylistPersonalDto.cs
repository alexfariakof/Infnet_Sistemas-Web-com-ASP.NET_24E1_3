using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Account.Dto;
public class PlaylistPersonalDto : IValidatableObject
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public IList<MusicDto> Musics { get; set; } = new List<MusicDto>();

    [JsonIgnore]
    public Guid CustumerId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var playlist = (PlaylistPersonalDto)validationContext.ObjectInstance;

        if (validationContext.Items.TryGetValue("HttpMethod", out var httpMethod) && httpMethod is string method)
        {
            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(playlist.Name))
            {
                yield return new ValidationResult("Name is required for create a playlist.", new[] { nameof(Name) });

                if (playlist.Musics != null && playlist.Musics.Any(m => m.Id == null))
                {
                    yield return new ValidationResult("Musics[0].Id is required for create musics in playlist.", new[] { nameof(Musics) });
                }
            }

            if (method.Equals("PUT", StringComparison.OrdinalIgnoreCase) && playlist.Id == null)
            {
                yield return new ValidationResult("ID is required for update a playlist.", new[] { nameof(Id) });

                if (playlist.Musics != null && playlist.Musics.Any(m => string.IsNullOrEmpty(m.Id.ToString())))
                {
                    yield return new ValidationResult("Musics[0].Id is required for update musics in playlist.", new[] { nameof(Musics) });
                }
            }

            if (method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) && playlist.Id == null)
            {
                yield return new ValidationResult("ID is required for delete a playlist.", new[] { nameof(Id) });
            }
        }
    }
}
