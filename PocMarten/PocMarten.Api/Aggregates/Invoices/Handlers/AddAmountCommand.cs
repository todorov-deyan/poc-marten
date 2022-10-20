using MediatR;
using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Handlers
{
    public record AddAmountCommand(decimal amount) : IRequest<Guid>;

    public class AddAmountInvoiceHandler : IRequestHandler<AddAmountCommand, Guid>
    {
        private readonly InvoiceRepository _invoiceRepository;
        public AddAmountInvoiceHandler(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task<Guid> Handle(AddAmountCommand request, CancellationToken cancellationToken)
        {
            InvoiceModel invoice = new InvoiceModel();

            var events = new List<IEventState>
            {
                new NetAmountValue(request.amount),
                new GrossAmountValue(request.amount)
            };

            await _invoiceRepository.Add(invoice, events, cancellationToken);
            return invoice.Id;
        }
    }
}
