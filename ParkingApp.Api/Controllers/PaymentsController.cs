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
    PaymentCalculator calculator) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<ActionResult<PaymentDto>> Calculate([FromBody] CalculatePaymentRequest request)
    {
        ParkingArea? area = await areaRepository.GetByIdAsync(request.ParkingAreaId);
        if (area == null) return NotFound("Id not found.");

        try
        {
            ParkingTime parkingTime = new ParkingTime(request.StartTime, request.EndTime);

            decimal amountUSD = calculator.Calculate(area, parkingTime);

            PaymentDto result = new()
            {
                Amount = amountUSD,
                Currency = CurrencyCodes.USD
            };

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
