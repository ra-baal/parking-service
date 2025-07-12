namespace ParkingApp.Api.Models;

public class PaymentResponse
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public Dictionary<string, decimal>? ConvertedAmounts { get; set; } = new();
}
