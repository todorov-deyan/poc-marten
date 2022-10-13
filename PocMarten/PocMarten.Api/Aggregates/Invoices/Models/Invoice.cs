using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Models
{
    public class Invoice : Aggregate
    {
        public double Amount { get; set; }

        public InvoiceStatus Status { get; private set; }

        public string DateIssued { get; private set; }

        public void Apply(IssuedAt @event)
        {
            Amount = @event.Amount;
        }
    }
}
