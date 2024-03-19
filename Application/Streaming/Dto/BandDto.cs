using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dto;
public class BandDto
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Backdrop { get; set; }
    public AlbumDto Album { get; set; }
}