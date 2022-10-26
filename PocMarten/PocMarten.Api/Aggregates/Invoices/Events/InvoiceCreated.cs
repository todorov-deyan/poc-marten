using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public record InvoiceCreated(Guid Id, decimal Amount, AmountType Status, DateTimeOffset CreatedAt) : IEventState;
}
