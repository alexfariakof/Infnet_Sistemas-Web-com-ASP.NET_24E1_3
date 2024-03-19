using System.Text.Json.Serialization;

namespace Application.Transactions.Dto;
public class CardDto
{
    [JsonIgnore]
    public decimal Limit { get; set; }
    public string? Number { get; set; }
    public DateTime? Validate { get; set; }
    public string? CVV { get; set; }

}