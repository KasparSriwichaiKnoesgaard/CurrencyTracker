using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyTrackerApi.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyData> GetCurrencyData()
        {
            // TODO: Implement logic to fetch currency data from the endpoint
            // Endpoint https://www.nationalbanken.dk/api/currencyratesxml?lang=da

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