using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Handlers;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Notifications;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;

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
            await _mediator.Send((new TemperatureChangedCommand(streamId, temperatureChange)));

            await _mediator.Publish(new TemperatureNotifyCommand(5), cancellationToken);

           return CreatedAtAction("Get", "WeatherForecast", new { streamId = streamId }, new { streamId = streamId });

        }

    }
}
