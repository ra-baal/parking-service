namespace ParkingApp.Api.Models;

public class PaymentResultDto
{
    public decimal AmountUSD { get; set; }
    public decimal? AmountEUR { get; set; }
    public decimal? AmountPLN { get; set; }
}
