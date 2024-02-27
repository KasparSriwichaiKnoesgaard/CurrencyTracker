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
        var currencyData = await _currencyService.GetCurrencyData();
        return Ok(currencyData);
    }
}
