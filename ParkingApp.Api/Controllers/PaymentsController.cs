using Microsoft.AspNetCore.Mvc;
using ParkingApp.Api.Models;
using ParkingApp.Domain.Services;
using ParkingApp.Domain.Entities;
using ParkingApp.Infrastructure.Repositories;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(
    IParkingAreaRepository areaRepository,
    ICurrencyConverter currencyConverter) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<ActionResult<PaymentResultDto>> Calculate([FromBody] PaymentRequest request)
    {
        DateTimeOffset now = DateTimeOffset.Now;

        ParkingArea? area = await areaRepository.GetByIdAsync(request.ParkingAreaId);
        if (area == null) return NotFound("Id not found.");

        TimeSpan duration = request.EndTime - request.StartTime;
        if (duration.TotalMinutes <= 0)
            return BadRequest("Time invalid.");

        decimal totalCost = 0;
        DateTimeOffset current = request.StartTime;

        while (current < request.EndTime)
        {
            DateTimeOffset nextHour = current.AddHours(1);
            if (nextHour > request.EndTime)
                nextHour = request.EndTime;

            bool isWeekend = current.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
            decimal rate = isWeekend ? area.WeekendRate : area.WeekdayRate;
            double hourFraction = (nextHour - current).TotalHours;

            totalCost += rate * (decimal)hourFraction;
            current = nextHour;
        }

        decimal finalUsd = Math.Round((decimal)totalCost * (1 - (decimal)area.DiscountPercentage / 100), 2);

        DateOnly? rateDate = request.EndTime.Date < now.Date ? DateOnly.FromDateTime(request.EndTime.Date) : null;
        Dictionary<string, decimal> converted = await currencyConverter.ConvertAsync(finalUsd, "USD", rateDate);

        PaymentResultDto result = new PaymentResultDto
        {
            AmountUSD = finalUsd,
            AmountEUR = converted.ContainsKey("EUR") ? converted["EUR"] : null,
            AmountPLN = converted.ContainsKey("PLN") ? converted["PLN"] : null
        };

        return Ok(result);
    }
}
