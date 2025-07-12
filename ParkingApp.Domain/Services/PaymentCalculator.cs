using ParkingApp.Domain.Entities;
using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Domain.Services;

public static class PaymentCalculator
{
    public static decimal Calculate(
        ParkingArea area,
        ParkingTime time)
    {
        decimal rate = time.IsWeekend ? area.WeekendRate : area.WeekdayRate;
        decimal total = rate * time.TotalHoursRoundedUp();
        decimal discountAmount = total * ((decimal)area.DiscountPercentage / 100);
        return Math.Round(total - discountAmount, 2);
    }
}
