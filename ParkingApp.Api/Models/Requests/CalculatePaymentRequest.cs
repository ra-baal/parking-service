using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Api.Models.Requests;

public class CalculatePaymentRequest
{
    [Required]
    public required string ParkingAreaId { get; set; }

    [Required]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    public DateTimeOffset EndTime { get; set; }
}
