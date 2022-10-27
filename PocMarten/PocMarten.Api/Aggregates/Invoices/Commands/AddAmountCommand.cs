using Ardalis.Result;
using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Commands
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
            InvoiceCreated invoiceCreated = new InvoiceCreated(

                Id : Guid.NewGuid(),
                Amount : request.amount,
                Status : AmountType.None,
                CreatedAt : DateTimeOffset.UtcNow
            );

            var newInvoice = InvoiceModel.Create(invoiceCreated);

            var events = new List<IEventState>
            {
                new NetAmountValue(invoiceCreated.Amount, invoiceCreated.CreatedAt),
                new GrossAmountValue(invoiceCreated.Amount, invoiceCreated.CreatedAt)
            };

            await _invoiceRepository.Add(newInvoice, events, cancellationToken: cancellationToken);

            return Result.Success(newInvoice);
        }
    }
}
