namespace ParkingApp.Api.Models;

public class UpdateParkingAreaRequest
{
    public string Name { get; set; } = null!;
    public decimal WeekdayRate { get; set; }
    public decimal WeekendRate { get; set; }
    public double DiscountPercentage { get; set; }
}
