using System.Net.Http.Json;
using ParkingApp.Domain.Services;
using ParkingApp.Domain.Constants;

namespace ParkingApp.Infrastructure.Services;

public class CurrencyConverter(
    HttpClient httpClient,
    ExchangeRatesApiSettings settings) : ICurrencyConverter
{
    public async Task<Dictionary<string, decimal>> ConvertAsync(decimal amount, string currencyBase, DateOnly? rateDate)
    {
        // Changing base currency is not available in free version of the API.

        string url = GetUrl(rateDate);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"API: {ex.Message}", ex);
        }

        ExchangeRateResponse? result = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();
        Dictionary<string, decimal> converted = new();

        if (result?.Rates is not null)
        {
            foreach ((string currency, decimal rate) in result.Rates)
            {
                converted[currency] = Math.Round(amount * rate, 2);
            }
        }

        return converted;
    }

    private string GetUrl(DateOnly? rateDate)
    {
        string dateSegment = rateDate.HasValue ? rateDate.Value.ToString("yyyy-MM-dd") : "latest";

        Uri baseUri = new(settings.BaseUrl);
        Uri fullUri = new(baseUri, dateSegment);

        string symbols = string.Join(",", CurrencyCodes.All);
        UriBuilder builder = new(fullUri)
        {
            Query = $"access_key={settings.ApiAccessKey}&symbols={symbols}"
        };

        string url = builder.ToString();
        return url;
    }

    private class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }

}

