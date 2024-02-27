using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyTrackerApi.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://www.nationalbanken.dk/api/") };
        }

        public async Task<CurrencyData> GetCurrencyData()
        {
            // TODO: Implement logic to fetch currency data from the endpoint
            // Endpoint https://www.nationalbanken.dk/api/currencyratesxml?lang=da
            var response = await _httpClient.GetAsync("currencyratesxml?lang=da");

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