using PocMarten.Api.Aggregates.Invoices.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Models
{
    public class InvoiceModel : Aggregate
    {
        public double Amount { get; private set; }
        public AmountType Status { get; private set; }

        public string DateIssued { get; private set; }


        public InvoiceModel()
        {
            Id = Guid.NewGuid();
        }

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
