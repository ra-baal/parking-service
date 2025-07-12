using System.Net.Http.Json;
using ParkingApp.Domain.Services;

namespace ParkingApp.Infrastructure.Services;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "YOUR_API_KEY"; // Secure via secrets manager or environment variable
    private const string BaseUrl = "https://api.apilayer.com/exchangerates_data";

    public CurrencyConverter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, decimal>> ConvertAsync(decimal amount, string baseCurrency, DateTime date)
    {
        string formattedDate = date.ToString("yyyy-MM-dd");
        string url = $"{BaseUrl}/{formattedDate}?base={baseCurrency}&symbols=EUR,PLN";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("apikey", ApiKey);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
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
