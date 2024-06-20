﻿using Application.Transactions.Dto;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Streaming.Dto;
public class MerchantDto
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }

    [Required]
    public string? CPF { get; set; }

    [Required]
    public string? CNPJ { get; set; }

    [Required]
    public string? Phone { get; set; }

    [Required]
    public AddressDto? Address { get; set; }

    public Guid FlatId { get; set; }

    [Required]
    public CardDto? Card { get; set; }
}