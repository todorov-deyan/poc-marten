﻿using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountOverdrafted : AccountBase
    {
        public AccountOverdrafted()
        {
            Time = DateTimeOffset.UtcNow;
        }
    }
}
