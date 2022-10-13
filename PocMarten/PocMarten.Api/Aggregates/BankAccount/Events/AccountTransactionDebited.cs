using PocMarten.Api.Aggregates.BankAccount.Model;

namespace PocMarten.Api.Aggregates.BankAccount.Events
{
    public class AccountTransactionDebited : Transaction
    {
        public override void Apply(Account account)
        {
            account.Balance -= Amount;
        }

        public AccountTransactionCredited ToCredit()
        {
            return new AccountTransactionCredited
            {
                Amount = Amount,
                To = From,
                From = To,
                Description = Description
            };
        }
    }
}
