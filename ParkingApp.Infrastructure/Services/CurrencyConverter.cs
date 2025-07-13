using System.Net.Http.Json;
using ParkingApp.Domain.Services;

namespace ParkingApp.Infrastructure.Services;

public class CurrencyConverter(
    HttpClient httpClient,
    ExchangeRatesApiSettings settings) : ICurrencyConverter
{
    private string GetUrl(DateOnly? rateDate)
    {
        string dateSegment = rateDate.HasValue ? rateDate.Value.ToString("yyyy-MM-dd") : "latest";

        Uri baseUri = new(settings.BaseUrl);
        Uri fullUri = new(baseUri, dateSegment);

        UriBuilder builder = new(fullUri)
        {
            Query = $"access_key={settings.ApiAccessKey}&symbols=EUR,PLN,USD"
        };

        string url = builder.ToString();
        return url;
    }

    public async Task<Dictionary<string, decimal>> ConvertAsync(decimal amount, string currencyBase, DateOnly? rateDate)
    {
        // Changing base currency is not available in free version on the API.

        string url = GetUrl(rateDate);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        ExchangeRateResponse? result = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();
        Dictionary<string, decimal> converted = new Dictionary<string, decimal>();

        if (result?.Rates is not null)
        {
            foreach ((string currency, decimal rate) in result.Rates)
            {
                converted[currency] = Math.Round(amount * rate, 2);
            }
        }

        return converted;
    }

    private class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}
