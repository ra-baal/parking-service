using Raven.Client.Documents;

namespace ParkingApp.Infrastructure.RavenDb;

public class RavenDbFactory
{
    private readonly RavenDbSettings _settings;

    public RavenDbFactory(RavenDbSettings settings)
    {
        _settings = settings;
    }

    public IDocumentStore Create()
    {
        DocumentStore store = new DocumentStore
        {
            Urls = _settings.Urls,
            Database = _settings.DatabaseName
        };

        store.Initialize();
        return store;
    }
}
