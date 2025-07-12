using Microsoft.AspNetCore.Mvc;
using ParkingApp.Api.Models;
using ParkingApp.Domain.Entities;
using ParkingApp.Infrastructure.Repositories;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingAreasController : ControllerBase
{
    private readonly IParkingAreaRepository _repository;

    public ParkingAreasController(IParkingAreaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkingAreaDto>>> GetAll()
    {
        IEnumerable<ParkingArea> areas = await _repository.GetAllAsync();
        return Ok(areas.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingAreaDto>> GetById(string id)
    {
        string parkingAreaId = $"ParkingAreas/{id}";

        ParkingArea? area = await _repository.GetByIdAsync(parkingAreaId);
        if (area == null) return NotFound();
        return Ok(ToDto(area));
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateParkingAreaRequest request)
    {
        ParkingArea newArea = new ParkingArea
        {
            Name = request.Name,
            WeekdayRate = request.WeekdayRate,
            WeekendRate = request.WeekendRate,
            DiscountPercentage = request.DiscountPercentage
        };

        await _repository.AddAsync(newArea);
        return CreatedAtAction(nameof(GetById), new { id = newArea.Id }, ToDto(newArea));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateParkingAreaRequest request)
    {
        string parkingAreaId = $"ParkingAreas/{id}";

        ParkingArea? existing = await _repository.GetByIdAsync(parkingAreaId);
        if (existing == null) return NotFound();

        existing.Name = request.Name;
        existing.WeekdayRate = request.WeekdayRate;
        existing.WeekendRate = request.WeekendRate;
        existing.DiscountPercentage = request.DiscountPercentage;

        await _repository.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        string parkingAreaId = $"ParkingAreas/{id}";

        ParkingArea? existing = await _repository.GetByIdAsync(parkingAreaId);
        if (existing == null) return NotFound();

        await _repository.DeleteAsync(parkingAreaId);
        return NoContent();
    }

    private static ParkingAreaDto ToDto(ParkingArea area) => new()
    {
        Id = area.Id,
        Name = area.Name,
        WeekdayRate = area.WeekdayRate,
        WeekendRate = area.WeekendRate,
        DiscountPercentage = area.DiscountPercentage
    };
}
