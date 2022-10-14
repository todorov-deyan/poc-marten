namespace PocMarten.Api.Aggregates.BankAccount.ModelDto
{
    public class AccountDto
    {
        public string Owner { get; set; }
        public decimal Balance { get; set; } = 0;
        public string Description { get; set; }
    }
}
