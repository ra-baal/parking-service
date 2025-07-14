using ParkingApp.Domain.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ParkingApp.Infrastructure.Repositories;

public class ParkingAreaRepository(
    IDocumentStore store) : IParkingAreaRepository
{
    public async Task<IEnumerable<ParkingArea>> GetAllAsync()
    {
        using IAsyncDocumentSession session = store.OpenAsyncSession();
        return await session.Query<ParkingArea>().ToListAsync();
    }

    public async Task<ParkingArea?> GetByIdAsync(string id)
    {
        using IAsyncDocumentSession session = store.OpenAsyncSession();
        return await session.LoadAsync<ParkingArea>(id);
    }

    public async Task AddAsync(ParkingArea area)
    {
        using IAsyncDocumentSession session = store.OpenAsyncSession();
        await session.StoreAsync(area);
        await session.SaveChangesAsync();
    }

    public async Task UpdateAsync(ParkingArea area)
    {
        using IAsyncDocumentSession session = store.OpenAsyncSession();
        session.Advanced.Evict(area); 
        await session.StoreAsync(area); 
        await session.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        using IAsyncDocumentSession session = store.OpenAsyncSession();
        session.Delete(id);
        await session.SaveChangesAsync();
    }
}
