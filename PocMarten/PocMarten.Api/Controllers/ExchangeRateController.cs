using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Commands;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Queries;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private static readonly Random Random = new();
        private readonly IMediator _mediator;
        private readonly ILogger<ExchangeRateController> _logger;

        public ExchangeRateController(IMediator mediator, ILogger<ExchangeRateController> logger)
        {            
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{streamId:guid}", Name = "GetExchangeRate")]
        public async Task<ActionResult<ExchangeRateDetails>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetExchangeRateByIdQuery(streamId), cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost(Name = "PostExchangeRate")]
        public async Task<ActionResult> Post(decimal currentExchangeRate, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new CurrentExchangeRateCommand(currentExchangeRate), cancellationToken);

            return CreatedAtAction("Get", new { streamId = result.Id }, new { streamId = result.Id });
        }
    }
}
