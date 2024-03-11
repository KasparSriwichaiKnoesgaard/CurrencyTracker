using CurrencyTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CurrencyTrackerApi.Controllers;

[ApiController]
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly CurrencyService _currencyService;

    public CurrencyController(ILogger<CurrencyController> logger, CurrencyService currencyService)
    {
        _logger = logger;
        _currencyService = currencyService;
    }

    [HttpGet("exchange-rate")]
    public async Task<IActionResult> GetExchangeRate()
    {
        var currencyData = await _currencyService.GetAllCurrencies();
        return Ok(currencyData);
    }

    [HttpGet("exchange-rate/{currencyCode}")]
    public async Task<IActionResult> GetExchangeRateForCurrency(string currencyCode)
    {
        try
        {
            string upperCaseCurrencyCode = currencyCode.ToUpper();
            var currencyData = await _currencyService.GetCurrencyByCode(upperCaseCurrencyCode);
            return Ok(currencyData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching exchange rate for currency code: {currencyCode}");
            return NotFound($"Currency code '{currencyCode}' not found.");
        }
    }
}
