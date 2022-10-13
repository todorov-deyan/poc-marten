using Microsoft.AspNetCore.Routing.Constraints;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class InvoiceDateStarted : IEventState
    {
        public double Amount { get; set; }

        public InvoiceDateStarted(double amount)
        {
            Amount = amount;
        }
    }
}
