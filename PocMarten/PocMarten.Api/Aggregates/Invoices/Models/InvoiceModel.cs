using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Common.EventSourcing;
using System.Text.Json.Serialization;

namespace PocMarten.Api.Aggregates.Invoices.Models
{
    public class InvoiceModel : Aggregate
    {
        public decimal Amount { get; set; }
        public AmountType Status { get; private set; }

        public DateTimeOffset DateIssued { get; private set; }

        public void Apply(NetAmountValue @event)
        {
            Amount = @event.NetAmount;
            Status = AmountType.NetAmountValue;
        }

        public void Apply(GrossAmountValue @event)
        {
            Amount = @event.GrossAmount;
            Status = AmountType.GrossAmountValue;
        }
    }
}
