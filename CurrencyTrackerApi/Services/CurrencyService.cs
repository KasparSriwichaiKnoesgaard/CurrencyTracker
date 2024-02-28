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

        public async Task<CurrencyData> GetCurrencyData(string currencyCode)
        {
            var response = await _httpClient.GetAsync(_currencyRatesEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var xmlData = await response.Content.ReadAsStringAsync();
                return GetCurrencyByCode(xmlData, currencyCode);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        private CurrencyData GetCurrencyByCode(string xmlData, string currencyCode)
        {
            XDocument xdoc = XDocument.Parse(xmlData);
            var currencyElement = xdoc.Descendants("currency")
                                      .FirstOrDefault(currency => (string)currency.Attribute("code") == currencyCode);

            if (currencyElement != null)
            {
                var rate = double.Parse(currencyElement.Attribute("rate").Value);
                return new CurrencyData(currencyCode, rate);
            }
            else
            {
                throw new InvalidOperationException($"Currency code '{currencyCode}' not found.");
            }
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