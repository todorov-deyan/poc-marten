using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountOverdraft : IEventState
    {
        public AccountOverdraft()
        {
            Time = DateTimeOffset.UtcNow;
        }

        public string Description { get; set; }
        public DateTimeOffset Time { get; set; }
        public decimal Amount { get; set; }
    }
}
