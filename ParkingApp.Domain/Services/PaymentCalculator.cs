using ParkingApp.Domain.Entities;
using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Domain.Services;

public class PaymentCalculator
{
    public decimal Calculate(
        ParkingArea area,
        ParkingTime time)
    {
        decimal rate = time.IsWeekend ? area.Rate.WeekendRate : area.Rate.WeekdayRate;
        int hours = (int)Math.Ceiling(time.TotalDuration.TotalHours);
        decimal total = rate * hours;
        decimal discountAmount = total * (area.DiscountPercentage / 100);

        decimal result = Math.Round(total - discountAmount, 2);
        return result;
    }
}
