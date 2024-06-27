﻿using Domain.Core.Aggreggates;

namespace Application.Streaming.Dto;
public class MusicDto : BaseDto
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public int Duration { get; set; } = 0;
    public Guid FlatId { get; set; }
    public Guid AlbumId { get; set; }
    public Guid GenreId {get; set; }
    public string? AlbumBackdrop { get; set; }
    public string? AlbumName { get; set; }
    public Guid BandId { get; set; }
    public string? BandBackDrop { get; set; }
    public string? BandName{ get; set; }
    public string? BandDescription { get; set; }
}