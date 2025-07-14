using ParkingApp.Api.Models.Dtos;
using ParkingApp.Api.Models.Requests;
using ParkingApp.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using ParkingApp.Domain.Constants;
using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController(
    CurrencyConverter currencyConverter) : ControllerBase
{
    [HttpPost("convert")]
    public async Task<ActionResult<PaymentDto>> Convert([FromBody] ConvertCurrencyRequest request)
    {
        DateTimeOffset now = DateTimeOffset.Now;

        if (!CurrencyCodes.Exists(request.FromCurrency) || !CurrencyCodes.Exists(request.ToCurrency))
            return BadRequest();

        DateOnly? rateDate = DateOnly.FromDateTime(request.Date.Date) < DateOnly.FromDateTime(now.Date)
            ? DateOnly.FromDateTime(request.Date.Date)
            : null;

        decimal converted = await currencyConverter.ConvertAsync(request.Amount, request.FromCurrency, request.ToCurrency, rateDate);

        PaymentDto result = new()
        {
            Amount = converted,
            Currency = request.ToCurrency
        };

        return Ok(result);
    }
}
