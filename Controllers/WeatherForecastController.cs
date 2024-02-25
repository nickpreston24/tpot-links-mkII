using CodeMechanic.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TPOT_Links.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    public IActionResult Details(int id)
    {
        // return ControllerContext.MyDisplayRouteInfo(id);
        id.Dump("you called?");
        return Ok(id);
    }
    //
    // [HttpGet(Name = "GetIslandFiles")]
    // public int[] GetIslandFiles()
    // {
    //     return new int[] { 500 };
    //     // var results_array = solution(user_ids.ToArray(), file_names.ToArray(), file_types.ToArray());
    //     // return results_array;
    // }
}