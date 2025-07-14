using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Api.Models.Requests;

public class ConvertCurrencyRequest
{
    [Required]
    public required string FromCurrency { get; set; }

    [Required]
    public required string ToCurrency { get; set; }

    [Required]
    public required decimal Amount { get; set; }

    [Required]
    public DateTimeOffset Date { get; set; }
}
