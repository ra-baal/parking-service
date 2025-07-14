namespace ParkingApp.Api.Models.Requests;
using System.ComponentModel.DataAnnotations;

public class UpdateParkingAreaRequest
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Range(0, 1000)]
    public decimal WeekdayRate { get; set; }

    [Range(0, 1000)]
    public decimal WeekendRate { get; set; }

    [Range(0, 100)]
    public decimal DiscountPercentage { get; set; }
}
