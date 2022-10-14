namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountWithdrawed : AccountBase
    {
        public AccountWithdrawed()
        {
            Time = DateTimeOffset.UtcNow;
        }
    }
}
