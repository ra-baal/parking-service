using Raven.Client.Documents;

namespace ParkingApp.Infrastructure.RavenDb;

public static class RavenDbFactory
{
    public static IDocumentStore Create(string url, string database)
    {
        DocumentStore store = new DocumentStore
        {
            Urls = [url],
            Database = database
        };

        store.Initialize(); // Połącz.
        return store;
    }
}
