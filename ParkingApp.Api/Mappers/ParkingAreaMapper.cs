using ParkingApp.Api.Models.Dtos;
using ParkingApp.Domain.Entities;
using ParkingApp.Api.Models.Requests;
using ParkingApp.Domain.ValueObjects;

namespace ParkingApp.Api.Mappers;

public static class ParkingAreaMapper
{
    public static ParkingAreaDto ToDto(ParkingArea area)
    {
        ArgumentNullException.ThrowIfNull(area);

        return new ParkingAreaDto
        {
            Id = area.Id,
            Name = area.Name,
            WeekdayRate = area.Rate.WeekdayRate,
            WeekendRate = area.Rate.WeekendRate,
            DiscountPercentage = area.DiscountPercentage
        };
    }

    public static ParkingArea ToEntity(CreateParkingAreaRequest request)
    {
        return new ParkingArea
        {
            Name = request.Name,
            Rate = new ParkingRate(request.WeekdayRate, request.WeekendRate),
            DiscountPercentage = request.DiscountPercentage
        };
    }

    public static void UpdateEntity(ParkingArea area, UpdateParkingAreaRequest request)
    {
        area.Name = request.Name;
        area.Rate = new ParkingRate(request.WeekdayRate, request.WeekendRate);
        area.DiscountPercentage = request.DiscountPercentage;
    }
} 