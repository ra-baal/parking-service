using ParkingApp.Domain.Entities;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ParkingApp.Infrastructure.Repositories;

public class ParkingAreaRepository : IParkingAreaRepository
{
    private readonly IDocumentStore _store;

    public ParkingAreaRepository(IDocumentStore store)
    {
        _store = store;
    }

    public async Task<IEnumerable<ParkingArea>> GetAllAsync()
    {
        using IAsyncDocumentSession session = _store.OpenAsyncSession();
        return await session.Query<ParkingArea>().ToListAsync();
    }

    public async Task<ParkingArea?> GetByIdAsync(string id)
    {
        using IAsyncDocumentSession session = _store.OpenAsyncSession();
        return await session.LoadAsync<ParkingArea>(id);
    }

    public async Task AddAsync(ParkingArea area)
    {
        using IAsyncDocumentSession session = _store.OpenAsyncSession();
        await session.StoreAsync(area);
        await session.SaveChangesAsync();
    }

    public async Task UpdateAsync(ParkingArea area)
    {
        using IAsyncDocumentSession session = _store.OpenAsyncSession();
        session.Advanced.Evict(area); // optional: reset tracking
        await session.StoreAsync(area); // overwrite
        await session.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        using var session = _store.OpenAsyncSession();
        session.Delete(id);
        await session.SaveChangesAsync();
    }
}
