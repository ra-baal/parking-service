namespace ParkingApp.Api.Models.Dtos;

public class ParkingAreaDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal WeekdayRate { get; set; }
    public decimal WeekendRate { get; set; }
    public decimal DiscountPercentage { get; set; }
}
