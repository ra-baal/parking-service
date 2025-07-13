namespace ParkingApp.Infrastructure.RavenDb;

public class RavenDbSettings
{
    public required string[] Urls { get; init; }
    public required string DatabaseName { get; init; }
}
