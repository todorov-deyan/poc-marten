using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public record GrossAmountValue(decimal GrossAmount, DateTimeOffset DateIssued) : IEventState
    {
        private const decimal GROSSVALUE = 1.2m;
        public decimal GrossAmount { get; init; } = GrossAmount * GROSSVALUE;
    }
}
