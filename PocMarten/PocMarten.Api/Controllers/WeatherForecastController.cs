using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Weather.Commands;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Queries;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IMediator mediator, ILogger<WeatherForecastController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpGet("{streamId:guid}", Name = "GetWeatherForecast")]
        public async Task<ActionResult<WeatherForecast>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetWeatherByIdQuery(streamId), cancellationToken);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<ActionResult> Post(int currentTemperature, CancellationToken cancellationToken = default)
        {
            var result =  await _mediator.Send(new CurrentTemperatureCommand(currentTemperature), cancellationToken);
            if (!result.IsSuccess)
                return BadRequest();
           
            return CreatedAtAction("Get", new { streamId = result.Value.Id }, new {streamId = result.Value.Id });
        }

    }
}
    