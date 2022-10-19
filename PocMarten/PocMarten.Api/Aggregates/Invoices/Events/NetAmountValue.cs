using Marten.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class NetAmountValue : IEventState
    {
        public DateTimeOffset DateIssued { get; set; }
        public decimal NetAmount { get; set; }
        public NetAmountValue(decimal amount)
        {
            DateIssued = DateTimeOffset.UtcNow;
            NetAmount = amount;
        }
    }
}
