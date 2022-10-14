using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Events;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Repository;

using PocMarten.Api.Common.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private static readonly Random Random = new();
        private readonly ExchangeRateRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;

        public ExchangeRateController(ExchangeRateRepository repository, ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{streamId:guid}", Name = "GetExchangeRate")]
        public async Task<ActionResult<ExchangeRateDetails>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            var result = await _repository.Find(streamId, cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost(Name = "PostExchangeRate")]
        public async Task<ActionResult> Post(decimal currentExchangeRate, CancellationToken cancellationToken = default)
        {
            ExchangeRateDetails exchangeRate = new ExchangeRateDetails();
            exchangeRate.Id = Guid.NewGuid();

            List<IEventState> events = new();

            events.Add(new ExchangeRateYesterday(currentExchangeRate));

            var bitcoinLow = Random.Next(0, 1000);

            events.Add(new ExchangeRateLow(currentExchangeRate + bitcoinLow*(-1)));

            var bitcoinHigh = Random.Next(1, 1000);
            events.Add(new ExchangeRateHigh(currentExchangeRate + bitcoinHigh));

            await _repository.Add(exchangeRate, events, cancellationToken);

            return CreatedAtAction("Get", new { streamId = exchangeRate.Id }, new { streamId = exchangeRate.Id });
        }
    }
}
