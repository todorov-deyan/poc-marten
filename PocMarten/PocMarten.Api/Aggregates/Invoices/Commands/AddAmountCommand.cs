using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Handlers
{
    public record AddAmountCommand(decimal amount) : ICommandRequest<Result<InvoiceModel>>;

    public class AddAmountInvoiceHandler : ICommandHandler<AddAmountCommand, Result<InvoiceModel>>
    {
        private readonly InvoiceRepository _invoiceRepository;
        public AddAmountInvoiceHandler(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task<Result<InvoiceModel>> Handle(AddAmountCommand request, CancellationToken cancellationToken)
        {

            InvoiceCreated invoiceCreated = new()
            {
                Id = Guid.NewGuid(),
                Amount = request.amount,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            var newInvoice = InvoiceModel.Create(invoiceCreated);

            var events = new List<IEventState>
            {
                new NetAmountValue(request.amount),
                new GrossAmountValue(request.amount)
            };

            await _invoiceRepository.Add(newInvoice, events, cancellationToken: cancellationToken);

            return Result.Success(newInvoice);
        }
    }
}
