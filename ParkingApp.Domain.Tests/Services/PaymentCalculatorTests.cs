using System;
using Xunit;
using ParkingApp.Domain.Entities;
using ParkingApp.Domain.ValueObjects;
using ParkingApp.Domain.Services;

namespace ParkingApp.Tests.Domain.Services;

public class PaymentCalculatorTests
{
    [Theory]
    [InlineData("2025-07-15", "08:00", "11:30", 10.0, 20.0, 0, 40.0)]
    [InlineData("2025-07-13", "10:00", "11:15", 10.0, 15.0, 0, 30.0)]
    [InlineData("2025-07-12", "09:00", "09:30", 8.0, 12.0, 25, 9.00)]
    [InlineData("2025-07-14", "08:00", "08:01", 20.0, 25.0, 50, 10.0)]
    public void Calculate_ShouldReturnCorrectAmount(
        string dateStr,
        string startStr,
        string endStr,
        decimal weekdayRate,
        decimal weekendRate,
        decimal discount,
        decimal expected)
    {
        // Arrange.
        DateOnly date = DateOnly.Parse(dateStr);
        TimeOnly start = TimeOnly.Parse(startStr);
        TimeOnly end = TimeOnly.Parse(endStr);

        ParkingArea area = new()
        {
            Name = "Test Area",
            Rate = new ParkingRate(weekdayRate, weekendRate),
            DiscountPercentage = discount
        };

        PaymentCalculator calculator = new();

        ParkingTime time = new(date, start, end);

        // Act.
        decimal result = calculator.Calculate(area, time);

        // Assert.
        Assert.Equal(expected, result);
    }
}
