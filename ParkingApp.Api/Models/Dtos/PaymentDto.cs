namespace ParkingApp.Api.Models.Dtos;

public class PaymentDto
{
    public required decimal Amount { get; set; }
    public required string Currency {  get; set; }
}
