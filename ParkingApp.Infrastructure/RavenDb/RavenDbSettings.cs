namespace ParkingApp.Infrastructure.RavenDb;

public class RavenDbSettings
{
    public required string[] Urls { get; set; }
    public required string DatabaseName { get; set; }
}
