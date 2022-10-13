namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public enum AccountStatus
    {
        None     = 0,
        Created  = 1,
        Credited = 2,
        Debited  = 3,
        Overdraft   = 4,
        Closed   = 5,
    }
}
