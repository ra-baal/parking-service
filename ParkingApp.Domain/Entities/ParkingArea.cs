using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Domain.Entities;

public class ParkingArea
{
    public string Id { get; set; } = null!;
    public required string Name { get; set; }
    public required ParkingRate Rate { get; set; }
    public decimal DiscountPercentage { get; set; }
}
