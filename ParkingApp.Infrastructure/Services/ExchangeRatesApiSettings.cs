namespace ParkingApp.Infrastructure.Services;

public class ExchangeRatesApiSettings
{
    public required string ApiAccessKey { get; init; }
    public required string BaseUrl { get; init; }
}
