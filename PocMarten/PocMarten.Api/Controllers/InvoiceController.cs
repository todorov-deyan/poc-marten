using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Invoices.Handlers;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Queries;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator, ILogger<InvoiceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(Name = "CreateInvoices")]
        public async Task<ActionResult> Post(decimal amount, CancellationToken cancellationToken = default)
        {
            var id = await _mediator.Send(new AddAmountCommand(amount), cancellationToken);

            return CreatedAtAction("Get", new { streamId = id }, new { streamId = id });
        }

        [HttpGet("{streamId:guid}", Name = "Invoices")]
        public async Task<ActionResult<InvoiceModel>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetInvoiceByIdQuery(streamId), cancellationToken);
                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
