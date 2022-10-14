using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class TransactionProccessed : IEventState
    {
        public Guid To { get; set; }
        public Guid From { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public decimal Amount { get; set; }

        public TransactionProccessed()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
