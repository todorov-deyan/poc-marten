using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public abstract class AccountBase : IEventState
    {
        public string Description { get; set; }
        public DateTimeOffset Time { get; set; }
        public decimal Amount { get; set; }
    }
}
