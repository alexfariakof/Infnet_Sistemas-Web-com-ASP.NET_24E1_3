﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dto;
public class LoginDto
{    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }

    [Required]
    public UserType Type { get; set; }
}
public enum UserType
{
    Customer,
    Merchant
}

