using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Invoices.Commands;
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
        public async Task<ActionResult<InvoiceModel>> Post(decimal amount, CancellationToken cancellationToken = default)
        {
           if(!ModelState.IsValid)
                return BadRequest();

            var invoice = await _mediator.Send(new AddAmountCommand(amount), cancellationToken);

            if (!invoice.IsSuccess)
                return BadRequest();

            return CreatedAtAction("Get", new { streamId = invoice.Value.Id }, new { streamId = invoice.Value });
        }

        [HttpGet("{streamId:guid}", Name = "Invoices")]
        public async Task<ActionResult<InvoiceModel>> Get(Guid streamId, CancellationToken cancellationToken = default)
         {

            if (!ModelState.IsValid)
                return BadRequest();

            var invoice = await _mediator.Send(new GetInvoiceByIdQuery(streamId), cancellationToken);

            if (!invoice.IsSuccess)
                return BadRequest();

            return Ok(invoice.Value);
        }
    }
}
