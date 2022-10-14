using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public class Transaction : Aggregate
    {
        public Guid From { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset CreatedAt { get; init; }
        public decimal Amount { get; private set; }
        public  TransactionStatus Status { get; private set; }

        protected Transaction(TransactionStarted @event)
        {
            Id = @event.AccountId;
            Description = @event.Description;
            CreatedAt = @event.CreatedAt;

            Status = TransactionStatus.Started;
        }

        protected Transaction(TransactionProccessed @event)
        {
            Id = @event.To;
            From = @event.From;
            Amount = @event.Amount;
            Description = @event.Description;
            CreatedAt = @event.CreatedAt;

            Status = TransactionStatus.Proccessed;
        }

        protected Transaction(TransactionComplated @event)
        {
            Id = @event.AccountId;
            Description = @event.Description;
            CreatedAt = @event.CreatedAt;

            Status = TransactionStatus.Complated;
        }

        public static Transaction Create(TransactionStarted @event)
        {
            return new Transaction(@event);
        }
        
        public static Transaction Create(TransactionProccessed @event)
        {
            return new Transaction(@event);
        }

        public static Transaction Create(TransactionComplated @event)
        {
            return new Transaction(@event);
        }
    }
}
