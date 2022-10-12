using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Weather;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Respository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly WeatherRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(WeatherRepository repository, ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet("{streamId}", Name = "GetWeatherForecast")]
        public async Task<ActionResult<WeatherForecast>> GetMarten(Guid streamId, CancellationToken cancellationToken = default)
        {
            var result = await _repository.Find(streamId, cancellationToken); 

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<ActionResult> Post(int currentTemperature, CancellationToken cancellationToken = default)
        {
            WeatherForecast weatherForecast = new WeatherForecast();
            weatherForecast.Id = Guid.NewGuid();

            List<IEventState> events = new();
            events.Add(new TemperatureMonitoringStarted(currentTemperature));
          
            await _repository.Add(weatherForecast, events, cancellationToken);

            return Ok();
        }

    }
}
    