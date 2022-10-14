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

        [HttpPost(Name = "CreateInvoices")]
        public async Task<ActionResult> Post(double amount, CancellationToken cancellationToken = default)
        {
            InvoiceModel invoice = new InvoiceModel();
            invoice.Id = Guid.NewGuid();

            var events = new List<IEventState>
            {
                new NetAmountValue(amount),
                new GrossAmountValue(amount)
            };

            await _repository.Add(invoice, events, cancellationToken);

            return CreatedAtAction("Post", new { streamId = invoice.Id }, new { streamId = invoice.Id });
        }

        [HttpGet("{streamId:guid}", Name = "Invoices")]
        public async Task<ActionResult<InvoiceModel>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.Find(streamId, cancellationToken);
                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
