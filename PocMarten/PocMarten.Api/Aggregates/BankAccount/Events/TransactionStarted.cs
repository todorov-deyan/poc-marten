using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class TransactionStarted : IEventState
    {
        public string Owner { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; init; }

        public TransactionStarted()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
