namespace PocMarten.Api.Aggregates.BankAccount.ModelDto
{
    public class AccountTransferRequest
    { 
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
