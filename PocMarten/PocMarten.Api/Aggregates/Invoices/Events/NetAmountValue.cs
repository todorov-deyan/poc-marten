using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public record NetAmountValue(decimal NetAmount, DateTimeOffset DateIssued) : IEventState;

}
