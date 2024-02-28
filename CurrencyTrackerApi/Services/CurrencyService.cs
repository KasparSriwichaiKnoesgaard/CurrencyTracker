using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyTrackerApi.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _currencyRatesEndpoint;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _currencyRatesEndpoint = configuration["ApiSettings:CurrencyRatesEndpoint"]!;
        }

        public async Task<CurrencyData> GetCurrencyData()
        {
            var response = await _httpClient.GetAsync(_currencyRatesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var xmlString = await response.Content.ReadAsStringAsync();

                // TODO: Parse XML
                //CurrencyData currencyData = ParseXmlData(xmlString);
                CurrencyData currencyData = new(xmlString, 686.73);

                return currencyData;
            }
            else
            {
                // Handle error
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            // return a dummy data
            //return new CurrencyData("USD", 686.73);
        }
    }

    public class CurrencyData
    {
        // TODO: Handle non-nullable CurrencyCode
        public string CurrencyCode { get; set; }
        public double ExchangeRate { get; set; }

        public CurrencyData(string currencyCode, double exchangeRate)
        {
            CurrencyCode = currencyCode;
            ExchangeRate = exchangeRate;
        }

    }
}