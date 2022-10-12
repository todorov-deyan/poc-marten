﻿using Marten.Internal.CodeGeneration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather;
using PocMarten.Api.Common.EventSourcing;
using PocMarten.Api.Aggregates.Weather.Respository;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{streamId:guid}/temperature")]
    public class WeatherForecastTemperatureController : Controller
    {
        private readonly WeatherRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastTemperatureController(WeatherRepository repository, ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost(Name = "PostTemperature")]
        public async Task<ActionResult> Post(Guid streamId, int temperatureChange, CancellationToken cancellationToken = default)
        {

            WeatherForecast? forecast = await _repository.Find(streamId, cancellationToken);
            if (forecast is null)
                return NotFound();

            List<IEventState> events = new();

            switch(Math.Sign(temperatureChange))
            {
                case -1: events.Add(new TemperatureLow(temperatureChange));
                    break;
                case 1: events.Add(new TemperatureHigh(temperatureChange));
                    break;
                default: events.Add(new TemperatureHigh(temperatureChange));
                    break;
            };
            
            await _repository.Update(forecast.Id, events, cancellationToken);

            return Ok();
        }

    }
}