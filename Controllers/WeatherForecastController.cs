using Microsoft.AspNetCore.Mvc;
using Practice_Docker.Db;

namespace Practice_Docker.Controllers
{
    [ApiController]
    [Route("weatherforecasts")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository _forecast;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository repos)
        {
            _logger = logger;
            _forecast = repos;
        }

        [HttpGet(Name = "GetWeatherForecasts")]
        public IActionResult Get()
        {
           var forecasts = _forecast.GetAllForecasts();
           return Ok(forecasts);
        }

        [HttpGet("{id}",Name = "GetWeatherForecast")]
        public async Task<IActionResult> GetById(int id)
        {
            var forecast = await _forecast.GetWeatherForecast(id);
            return Ok(forecast);
        }

        [HttpPost]
        public async Task<IActionResult> AddForecast()
        {
            var forecast = new WeatherForecast() 
            {
                Date = DateTime.Now.AddDays(Random.Shared.Next(10)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            var response = await _forecast.AddForecast(forecast);

            if(response)
                return Ok("Added");

            return BadRequest("Try again");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForecast(int id)
        {
            var forecast = await _forecast.GetWeatherForecast(id);
            if (forecast == null)
                return NotFound("Forecast was not found");

            forecast.Date = DateTime.Now.AddDays(Random.Shared.Next(10));

            forecast.TemperatureC = Random.Shared.Next(-20, 55);

            forecast.Summary = Summaries[Random.Shared.Next(Summaries.Length)];

            var response = await _forecast.UpdateForecast(forecast);
            if (response)
                return Ok("Updated successfully");

            return BadRequest("Try again");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForecast(int id)
        {
            var forecast = await _forecast.GetWeatherForecast(id);
            if (forecast == null)
                return NotFound("Forecast was not found");

            var response = await _forecast.RemoveForecast(forecast);

            if (response)
                return NoContent();

            return BadRequest("Try again");
        }
    }
}