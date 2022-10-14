using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public class Transaction : Aggregate
    {
      
        public Guid To { get; set; }
        public Guid From { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public decimal Amount { get; set; }

        public  TransactionStatus Status { get; set; }
       
        public Transaction(TransactionStarted started)
        {
            CreatedAt = UpdatedAt = started.CreatedAt;
            Status = TransactionStatus.Started;
        }


        public void Apply(TransactionProccessed proccessed)
        {
            UpdatedAt = proccessed.Time;
            Amount = proccessed.Amount;
            Status = TransactionStatus.Proccessed;
        }

        public void Apply(TransactionComplated complate)
        {
            UpdatedAt = complate.CreatedAt;
            Status = TransactionStatus.Complated;
        }
    }
}
