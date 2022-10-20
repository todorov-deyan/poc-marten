using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public class Account : Aggregate
    {
        public string Owner { get; init; }
        public decimal Balance { get; private set; }

        private List<Transaction> _transactions = new List<Transaction>();
        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
     
        public bool IsOverdraftAllowed { get; init; }
        public AccountStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; private set; }

       // [JsonConstructor]
        private Account() 
        {
        }

        protected Account(AccountCreated @event) 
        {
            _ = @event ?? throw new ArgumentNullException(nameof(@event));

            if(Balance < 0)
                throw new ArgumentException(nameof(@event.StartingBalance));

            Id = Guid.NewGuid();
            Owner = @event.Owner;
            Balance = @event.StartingBalance;
            IsOverdraftAllowed = @event.IsOverdraftAllowed;
            CreatedAt = UpdatedAt = @event.CreatedAt;
            Status = AccountStatus.Created;
        }

        public static Account Create(AccountCreated @event)
        {
            return new Account(@event);
        }

        //public void Apply(AccountCreated @event)
        //{
        //    this.Balance += @event.StartingBalance;
        //    Status = AccountStatus.Created;
        //}

        public void Apply(AccountDebited @event)
        {
            this.Balance += @event.Amount;
            Status = AccountStatus.Debited;
        }

        public void Apply(AccountWithdrawed @event)
        {
            this.Balance -= @event.Amount;
            Status = AccountStatus.Withdrawed;
        }

        public void Apply(AccountOverdrafted @event)
        {
            this.Balance -= @event.Amount;

            Status = AccountStatus.Overdrafted;
        }

        public void Apply(AccountClosed @event)
        {
            Balance = @event.ClosingBalance;
            Status = AccountStatus.Closed;
        }
        
        public void Apply(TransactionProccessed @event)
        {
            Transaction tr = Transaction.Create(@event);
            _transactions.Add(tr);

            UpdatedAt = @event.CreatedAt;
            Balance += @event.Amount;

            Status = AccountStatus.TransactionProccessed;
        }

        public void Apply(TransactionComplated @event)
        {
            Transaction tr = Transaction.Create(@event);
            _transactions.Add(tr);

            UpdatedAt = @event.CreatedAt;

            Status = AccountStatus.TransactionComplated;
        }


        ////????????
        //public bool HasSufficientFunds(AccountDebited debit)
        //                => (Balance - debit.Amount) >= 0;
    }
}
