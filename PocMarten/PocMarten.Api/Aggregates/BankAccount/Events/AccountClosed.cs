﻿using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountClosed : IEventState
    {
        public AccountClosed()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public string Owner { get; set; }
        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; init; }
        public decimal ClosingBalance { get; set; } = 0;

        public void Apply(Account account)
        {
            account.Balance = ClosingBalance;
        }
    }
}