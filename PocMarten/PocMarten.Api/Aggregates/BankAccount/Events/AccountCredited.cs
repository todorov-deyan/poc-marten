using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountCredited : IEventState
    {
        public AccountCredited()
        {
            Time = DateTimeOffset.UtcNow;
        }

        public string Description { get; set; }

        public DateTimeOffset Time { get; init; }

        public decimal Amount { get; set; }
    }
}
