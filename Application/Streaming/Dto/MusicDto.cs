﻿using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dto;
public class MusicDto
{
    public Guid Id { get; set; }
    public String? Name { get; set; }
    public int Duration { get; set; } = 0;
    public Guid FlatId { get; set; }
    public Guid AlbumId { get; set; }
}