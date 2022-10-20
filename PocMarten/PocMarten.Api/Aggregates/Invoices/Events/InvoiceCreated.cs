using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class InvoiceCreated : IEventState
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public AmountType Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
