using ParkingApp.Domain.Entities;

namespace ParkingApp.Infrastructure.Repositories;

public interface IParkingAreaRepository
{
    Task<IEnumerable<ParkingArea>> GetAllAsync();
    Task<ParkingArea?> GetByIdAsync(string id);
    Task AddAsync(ParkingArea area);
    Task UpdateAsync(ParkingArea area);
    Task DeleteAsync(string id);
}
