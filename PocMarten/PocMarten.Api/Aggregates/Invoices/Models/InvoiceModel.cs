using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Common.EventSourcing;
using System.Text.Json.Serialization;

namespace PocMarten.Api.Aggregates.Invoices.Models
{
    public class InvoiceModel : Aggregate
    {
        public AmountInvoice Amount { get; set; }

        public AmountType Status { get; private set; }

        public DateTimeOffset DateIssued { get; private set; }

        private InvoiceModel()
        {
        }

        protected InvoiceModel(InvoiceCreated @event)
        {
            _ = @event ?? throw new ArgumentNullException(nameof(@event));

            Id = Guid.NewGuid();
            Amount = new AmountInvoice(@event.Amount);
            Status = @event.Status;
            DateIssued = @event.CreatedAt;
        }

        public static InvoiceModel Create(InvoiceCreated @event)
        {
            return new InvoiceModel(@event);
        }

        public void Apply(NetAmountValue @event)
        {
            Amount = new AmountInvoice(@event.NetAmount);
            Status = AmountType.NetAmountValue;
        }

        public void Apply(GrossAmountValue @event)
        {
            Amount = new AmountInvoice(@event.GrossAmount);
            Status = AmountType.GrossAmountValue;
        }
    }
}
