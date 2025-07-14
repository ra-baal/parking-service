using Microsoft.AspNetCore.Mvc;
using ParkingApp.Domain.Services;
using ParkingApp.Domain.Entities;
using ParkingApp.Infrastructure.Repositories;
using ParkingApp.Domain.ValueObjects;
using ParkingApp.Domain.Constants;
using ParkingApp.Api.Models.Dtos;
using ParkingApp.Api.Models.Requests;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(
    IParkingAreaRepository areaRepository,
    ICurrencyConverter currencyConverter) : ControllerBase
{
    private readonly PaymentCalculator calculator = new();

    [HttpPost("calculate")]
    public async Task<ActionResult<PaymentResultDto>> Calculate([FromBody] PaymentRequest request)
    {
        DateTimeOffset now = DateTimeOffset.Now;

        ParkingArea? area = await areaRepository.GetByIdAsync(request.ParkingAreaId);
        if (area == null) return NotFound("Id not found.");

        try
        {
            ParkingTime parkingTime = new ParkingTime(request.StartTime, request.EndTime);

            decimal finalUsd = calculator.Calculate(area, parkingTime);

            DateOnly? rateDate = parkingTime.Date < DateOnly.FromDateTime(now.Date) 
                ? DateOnly.FromDateTime(request.EndTime.Date) 
                : null;

            Dictionary<string, decimal> converted = await currencyConverter.ConvertAsync(finalUsd, CurrencyCodes.USD, rateDate);

            PaymentResultDto result = new PaymentResultDto
            {
                AmountUSD = finalUsd,
                AmountEUR = converted.ContainsKey(CurrencyCodes.EUR) ? converted[CurrencyCodes.EUR] : null,
                AmountPLN = converted.ContainsKey(CurrencyCodes.PLN) ? converted[CurrencyCodes.PLN] : null
            };

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
