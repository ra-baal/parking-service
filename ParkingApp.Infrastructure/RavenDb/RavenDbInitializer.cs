using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace ParkingApp.Infrastructure.RavenDb;

public static class RavenDbInitializer
{
    public static void EnsureDatabaseExists(IDocumentStore store, string databaseName)
    {
        DatabaseRecord dbRecord = new DatabaseRecord(databaseName);
        CreateDatabaseOperation operation = new CreateDatabaseOperation(dbRecord);

        try
        {
            store.Maintenance.Server.Send(operation);
        }
        catch (Raven.Client.Exceptions.ConcurrencyException)
        {
            // DB already exists — ignore
        }
    }
}
