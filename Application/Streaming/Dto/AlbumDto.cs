using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dto;
public class AlbumDto
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public Guid FlatId { get; set; }
    public string Backdrop { get; set; }
    public List<MusicDto> Musics { get; set; } = new List<MusicDto>();
}