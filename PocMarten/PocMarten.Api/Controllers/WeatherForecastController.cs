using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Weather;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Repository;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly WeatherRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(WeatherRepository repository, ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //        {
        //            Date = DateTime.Now.AddDays(index),
        //            TemperatureC = Random.Shared.Next(-20, 55),
        //            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //        })
        //        .ToArray();
        //}

        [HttpGet(Name = "GetMarten")]
        public async Task<ActionResult<WeatherForecast>> GetMarten(Guid streamId)
        {
            var result = await _repository.Find(new Guid("1aa8ff06-a2e7-4ec1-81b3-12ada4b8b199"));

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost(Name = "PostMarten")]
        public async Task<ActionResult> Post()
        {
            WeatherForecast weatherForecast = new WeatherForecast();
            weatherForecast.Id = Guid.NewGuid();

            List<object> events = new();

            events.Add((new TemperatureHigh(1)));
            events.Add((new TemperatureHigh(3)));
            events.Add((new TemperatureHigh(5)));
            events.Add((new TemperatureLow(2)));


            await _repository.Add(weatherForecast, events.ToArray());

            return Ok();
        }
    }
}
    