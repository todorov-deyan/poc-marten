using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public class AccountTransaction : Aggregate
    {
        public Guid To { get; set; }
        public Guid From { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Time { get; init; }
        public AccountStatus TransactionStatus { get; set; }

        public decimal Amount { get; set; }

        protected AccountTransaction()
        {
            Time = DateTimeOffset.UtcNow;
        }
        
        public void Apply(AccountTransactionCredited credited)
        {
            credited.Apply(this);
            TransactionStatus = AccountStatus.Debited;
        }

        public void Apply(AccountTransactionDebited debit)
        {
            debit.Apply(this);
            TransactionStatus = AccountStatus.Debited;
        }

    }
}
