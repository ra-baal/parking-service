using Microsoft.AspNetCore.Mvc;
using ParkingApp.Domain.Entities;
using ParkingApp.Infrastructure.Repositories;
using ParkingApp.Api.Mappers;
using ParkingApp.Api.Models.Dtos;
using ParkingApp.Api.Models.Requests;
using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingAreasController(
    IParkingAreaRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkingAreaDto>>> GetAll()
    {
        IEnumerable<ParkingArea> areas = await repository.GetAllAsync();
        return Ok(areas.Select(ParkingAreaMapper.ToDto));
    }

    [HttpGet("by-id")]
    public async Task<ActionResult<ParkingAreaDto>> GetById([FromQuery] string id)
    {
        ParkingArea? area = await repository.GetByIdAsync(id);
        if (area == null) return NotFound();

        ParkingAreaDto dto = ParkingAreaMapper.ToDto(area);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateParkingAreaRequest request)
    {
        ParkingArea newArea = new()
        {
            Name = request.Name,
            Rate = new ParkingRate(request.WeekdayRate, request.WeekendRate),
            DiscountPercentage = request.DiscountPercentage
        };
            
        await repository.AddAsync(newArea);
        return CreatedAtAction(nameof(GetById), new { id = newArea.Id }, ParkingAreaMapper.ToDto(newArea));
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromQuery] string id, [FromBody] UpdateParkingAreaRequest request)
    {
        ParkingArea? existing = await repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = request.Name;
        existing.Rate = new ParkingRate(request.WeekdayRate, request.WeekendRate);
        existing.DiscountPercentage = request.DiscountPercentage;

        await repository.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] string id)
    {
        ParkingArea? existing = await repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await repository.DeleteAsync(id);
        return NoContent();
    }
}
