using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class TransactionComplated : IEventState
    {
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; init; }

        public TransactionComplated()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
