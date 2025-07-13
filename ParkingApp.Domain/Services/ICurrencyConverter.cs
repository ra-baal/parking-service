namespace ParkingApp.Domain.Services;

public interface ICurrencyConverter
{
    Task<Dictionary<string, decimal>> ConvertAsync(decimal amount, string currencyBase, DateOnly? rateDate);
}
