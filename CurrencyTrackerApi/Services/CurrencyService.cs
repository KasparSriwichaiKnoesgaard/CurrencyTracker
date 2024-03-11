using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        // TODO: Change to getAll
        public async Task<CurrencyData> GetCurrencyData()
        {
            var xmlData = await FetchCurrencyData();
            return GetAllCurrencies(xmlData).FirstOrDefault(); //remove after change to getAll
        }

        public async Task<CurrencyData> GetCurrencyData(string currencyCode)
        {
            var xmlData = await FetchCurrencyData();
            return GetCurrencyByCode(xmlData, currencyCode);
        }

        private async Task<string> FetchCurrencyData()
        {
            var response = await _httpClient.GetAsync(_currencyRatesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        private List<CurrencyData> GetAllCurrencies(string xmlData)
        {
            XDocument xdoc = XDocument.Parse(xmlData);
            var currencyElements = xdoc.Descendants("currency");

            List<CurrencyData> currencyList = currencyElements.Select(currency => new CurrencyData(
                (string)currency.Attribute("code"),
                double.Parse(currency.Attribute("rate").Value, CultureInfo.InvariantCulture))).ToList();
            return currencyList;
        }

        private CurrencyData GetCurrencyByCode(string xmlData, string currencyCode)
        {
            var allCurrencies = GetAllCurrencies(xmlData);
            var currency = allCurrencies.FirstOrDefault(c => c.CurrencyCode == currencyCode);

            if (currency == null)
            {
                throw new InvalidOperationException($"Currency code '{currencyCode}' not found.");
            }

            return currency;
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