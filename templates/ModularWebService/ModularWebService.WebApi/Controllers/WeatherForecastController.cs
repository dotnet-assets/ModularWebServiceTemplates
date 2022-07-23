using Microsoft.AspNetCore.Mvc;

namespace ModularWebService.WebApi.Controllers;

[ApiController]
[Route("api/weather-forecast")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<WeatherForecastController> _logger;
}