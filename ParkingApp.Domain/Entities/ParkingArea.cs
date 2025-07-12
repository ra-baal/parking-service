namespace ParkingApp.Domain.Entities;

public class ParkingArea
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal WeekdayRate { get; set; }
    public decimal WeekendRate { get; set; }
    public double DiscountPercentage { get; set; } // e.g., 10 for 10%
}
