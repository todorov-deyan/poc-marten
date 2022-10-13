using Marten.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class IssuedAt : IEventState
    {
        public string DateIssued { get; set; }
        public double Amount { get; set; }
        public IssuedAt(double amount)
        {
            Amount = amount;
        }
    }
}
