using Marten.Events;
using Microsoft.CodeAnalysis.Differencing;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public class Account : Aggregate
    {

        public string Owner { get; set; }
        public decimal Balance { get; set; }

        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
     
        public bool IsOverdraftAllowed { get; set; }

        public AccountStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


        public Account()
        {
            
        }

        public void Apply(IEvent<AccountCreated> @event)
        {
            Id = @event.Data.AccountId;
            Owner = @event.Data.Owner;
            Balance = @event.Data.StartingBalance;
            CreatedAt = UpdatedAt = @event.Data.CreatedAt;

            Status = AccountStatus.Created;
        }

        public void Apply(AccountDebited debit)
        {
            this.Balance += debit.Amount;
            Status = AccountStatus.Debited;
        }

        public void Apply(AccountWithdrawed withdraw)
        {
            this.Balance -= withdraw.Amount;
            Status = AccountStatus.Withdrawed;
        }

        public void Apply(AccountOverdrafted overdraft)
        {
            this.Balance -= overdraft.Amount;//????

            Status = AccountStatus.Overdrafted;
        }

        public void Apply(AccountClosed closed)
        {
            closed.Apply(this);
            Status = AccountStatus.Closed;
        }

        //????????
        public bool HasSufficientFunds(AccountDebited debit)
                        => (Balance - debit.Amount) >= 0;
    }
}
