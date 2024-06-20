﻿using Domain.Core.Aggreggates;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Application.Streaming.Dto;
public class FlatDto : BaseDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    private decimal _value;

    [Required(ErrorMessage = "O campo Valor é obrigatório.")]
    public decimal Value
    {
        get => _value;
        set => _value = value;
    }

    // Propriedade para retornar o valor formatado como string
    public string FormattedValue
    {
        get => _value.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));
        set
        {
            if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"), out decimal result))
            {
                _value = result;
            }
            else
            {
                throw new ArgumentException("A string fornecida não é um valor de moeda válido.");
            }
        }
    }
}