namespace ParkingApp.Api.Models;

public class PaymentRequest
{
    public string ParkingAreaId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
}
