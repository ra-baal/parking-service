using System.Net.Http.Json;
using ParkingApp.Domain.Services;
using ParkingApp.Domain.Constants;

namespace ParkingApp.Infrastructure.Services;

public class CurrencyRateProvider(
    HttpClient httpClient,
    ExchangeRatesApiSettings settings) : ICurrencyRateProvider
{
    public async Task<Dictionary<string, decimal>> GetExchangeRateAsync(
        string _currencyBase, // Changing base currency is not available in free version of the API. Documantation: The default base currency is EUR.
        IReadOnlyCollection<string> targetCurrencies,
        DateOnly? rateDate)
    {
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
        Dictionary<string, decimal> rates = result.Rates;

        return rates;
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

