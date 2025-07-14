namespace ParkingApp.Domain.Services;

public interface ICurrencyRateProvider
{
    Task<Dictionary<string, decimal>> GetExchangeRateAsync(string currencyBase, IReadOnlyCollection<string> targetCurrencies, DateOnly? rateDate);
}
