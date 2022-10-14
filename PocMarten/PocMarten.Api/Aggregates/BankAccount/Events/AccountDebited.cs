using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountDebited : AccountBase
    {
        public AccountDebited()
        {
            Time = DateTimeOffset.UtcNow;
        }
    }
}
