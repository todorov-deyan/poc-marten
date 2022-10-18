using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Weather.Commands;
using PocMarten.Api.Aggregates.Weather.Notifications;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{streamId:guid}/temperature")]
    public class WeatherForecastTemperatureController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastTemperatureController(IMediator mediator, ILogger<WeatherForecastController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "PostTemperature")]
        public async Task<ActionResult> Post(Guid streamId, int temperatureChange, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send((new TemperatureChangedCommand(streamId, temperatureChange)), cancellationToken);
            if (!result.IsSuccess)
            {
                ActionResult actionResult = result.Status switch
                {
                    ResultStatus.NotFound => NotFound(),
                    ResultStatus.Error => BadRequest(),
                };
                return actionResult;
            }

            await _mediator.Publish(new TemperatureNotifyCommand(5), cancellationToken);

            return CreatedAtAction("Get", "WeatherForecast", new { streamId = streamId }, new { streamId = streamId });
        }

    }
}
