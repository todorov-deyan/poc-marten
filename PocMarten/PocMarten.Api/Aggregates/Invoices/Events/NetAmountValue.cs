using Marten.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class NetAmountValue : IEventState
    {
        public string DateIssued { get; set; }
        public double NetAmount { get; set; }
        public NetAmountValue(double amount)
        {
            DateIssued = DateTime.UtcNow.ToString("dd/MM/yyyy");
            NetAmount = amount;
        }
    }
}
