using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceRepository _repository;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(InvoiceRepository repository, ILogger<InvoiceController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost(Name = "PostInvoice")]
        public async Task<ActionResult> Post(double amount, CancellationToken cancellationToken = default)
        {
            Invoice invoice = new Invoice();
            invoice.Id = Guid.NewGuid();

            List<IEventState> events = new();

            events.Add(new InvoiceDateStarted(amount));

            await _repository.Add(invoice, events, cancellationToken);

            return CreatedAtAction("Post", new { streamId = invoice.Id }, new { streamId = invoice.Id });
        }
    }
}
