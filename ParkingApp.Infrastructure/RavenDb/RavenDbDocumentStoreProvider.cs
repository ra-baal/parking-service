using Raven.Client.Documents;

namespace ParkingApp.Infrastructure.RavenDb;

public static class RavenDbDocumentStoreProvider
{
    private static IDocumentStore? _store;

    public static IDocumentStore GetDocumentStore(string databaseName = "ParkingApp")
    {
        if (_store != null)
            return _store;

        _store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" }, // RavenDB server URL
            Database = databaseName
        };

        _store.Initialize();
        return _store;
    }
}
