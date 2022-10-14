using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountCreated : IEventState
    {
        public string Owner { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public decimal StartingBalance { get; set; } = 0;

        public bool IsOverdraftAllowed { get; set; }

        public AccountCreated()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
