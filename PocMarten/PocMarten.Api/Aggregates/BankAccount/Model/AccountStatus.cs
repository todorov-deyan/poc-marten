namespace PocMarten.Api.Aggregates.BankAccount.Model
{
    public enum AccountStatus
    {
        None       = 0,
        Created    = 1,
        Withdrawed = 2,
        Debited    = 3,
        Overdrafted= 4,
        Closed     = 5,
        TransactionStarted = 7,
        TransactionProccessed = 8,
        TransactionComplated = 9
    }
}
