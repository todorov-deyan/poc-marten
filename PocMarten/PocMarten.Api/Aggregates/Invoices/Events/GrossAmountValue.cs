using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class GrossAmountValue : IEventState
    {
        private const decimal GROSSVALUE = 1.2m;
        public DateTimeOffset DateIssued { get; set; }
        public decimal GrossAmount { get; set; }

        public GrossAmountValue(decimal grossAmount)
        {
            DateIssued = DateTimeOffset.Now;
            GrossAmount = grossAmount * GROSSVALUE;
        }
    }
}
