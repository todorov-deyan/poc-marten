using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public abstract class Transaction : IEventState
    {
        protected Transaction()
        {
            Time = DateTimeOffset.UtcNow;
        }

        public Guid To { get; set; }
        public Guid From { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Time { get; init; }

        public decimal Amount { get; set; }

        public abstract void Apply(AccountTransaction account);
    }
}
