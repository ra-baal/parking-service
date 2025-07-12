// ParkingApp.Infrastructure/Raven/RavenDbStartupService.cs
using Microsoft.Extensions.Hosting;
using ParkingApp.Infrastructure.RavenDb;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace ParkingApp.Infrastructure.Raven;

public class RavenDbStartupService : IHostedService
{
    private readonly IDocumentStore _store;
    private readonly RavenDbSettings _settings;

    public RavenDbStartupService(IDocumentStore store, RavenDbSettings settings)
    {
        _store = store;
        _settings = settings;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        string[] existing = _store.Maintenance.Server
            .Send(new GetDatabaseNamesOperation(0, 25));

        if (!existing.Contains(_settings.DatabaseName))
        {
            _store.Maintenance.Server.Send(
                new CreateDatabaseOperation(
                    new DatabaseRecord(_settings.DatabaseName)));
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
