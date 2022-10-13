using PocMarten.Api.Aggregates.BankAccount.Model;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountTransactionCredited : Transaction
    {
        public override void Apply(Account account)
        {
            account.Balance += Amount;
        }

        public AccountTransactionDebited ToDebit()
        {
            return new AccountTransactionDebited
            {
                Amount = Amount,
                To = From,
                From = To,
                Description = Description
            };
        }
    
    }
}
