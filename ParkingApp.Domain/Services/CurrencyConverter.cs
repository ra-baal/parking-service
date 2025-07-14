using ParkingApp.Domain.Constants;

namespace ParkingApp.Domain.Services;

public class CurrencyConverter(
    ICurrencyRateProvider rateProvider)
{
    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency, DateOnly? date = null)
    {
        if (fromCurrency == toCurrency)
            return amount;

        // Only EUR as a base currency. Additional calculations needed.
        Dictionary<string, decimal> rates = await rateProvider.GetExchangeRateAsync(
            currencyBase: CurrencyCodes.EUR,
            targetCurrencies: CurrencyCodes.All,
            rateDate: date);

        if (!rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
            throw new InvalidOperationException("Missing rates");

        decimal amountInEur = GetAmountInEur(amount, fromCurrency, rates);

        if (toCurrency == CurrencyCodes.EUR)
        {
            return Math.Round(amountInEur);
        }
        else
        {
            decimal toRate = rates[toCurrency];
            return Math.Round(amountInEur * toRate);
        }
    }

    private decimal GetAmountInEur(decimal amount, string fromCurrency, Dictionary<string, decimal> rates)
    {
        if (fromCurrency == CurrencyCodes.EUR)
        {
            return amount;
        }
        else
        {
            decimal fromRate = rates[fromCurrency];
            return amount / fromRate;
        }

    }
}
