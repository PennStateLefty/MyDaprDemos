using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Route("{location}")]
    public async Task<WeatherForecast> Get(string location, [FromServices] Dapr.Client.DaprClient client)
    {
        return await client.GetStateAsync<WeatherForecast>("statestore", location);
    }

    [HttpPost(Name = "CreateWeatherForecast")]
    [Dapr.Topic("pubsub", "new")]
    public async Task<ActionResult<WeatherForecast>> CreateForecast(WeatherForecast newForecast, [FromServices] Dapr.Client.DaprClient client)
    {
        await client.SaveStateAsync<WeatherForecast>("statestore", newForecast.Location, newForecast);
        return newForecast;
    }
}
